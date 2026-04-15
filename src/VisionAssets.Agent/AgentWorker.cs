using Microsoft.Extensions.Options;
using VisionAssets.Persistence;

namespace VisionAssets.Agent;

/// <summary>Loop principal: heartbeat, registo de execuções no SQLite (EPIC-002), coleta real em EPIC-003.</summary>
public sealed class AgentWorker : BackgroundService
{
    private readonly ILogger<AgentWorker> _logger;
    private readonly IOptionsMonitor<AgentOptions> _options;
    private readonly IMachineRepository _machine;
    private readonly IInventoryRunRepository _inventory;

    public AgentWorker(
        ILogger<AgentWorker> logger,
        IOptionsMonitor<AgentOptions> options,
        IMachineRepository machine,
        IInventoryRunRepository inventory)
    {
        _logger = logger;
        _options = options;
        _machine = machine;
        _inventory = inventory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "VisionAssets Agent em execução (EPIC-001/002). Coleta WMI/Registry em EPIC-003.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var minutes = Math.Clamp(_options.CurrentValue.HeartbeatIntervalMinutes, 1, 24 * 60);
            var delay = TimeSpan.FromMinutes(minutes);

            _logger.LogInformation("Heartbeat: próximo ciclo em {Interval} minutos.", minutes);

            await RecordInventoryRunAsync(stoppingToken).ConfigureAwait(false);

            try
            {
                await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }

        _logger.LogInformation("VisionAssets Agent encerrando.");
    }

    private async Task RecordInventoryRunAsync(CancellationToken cancellationToken)
    {
        string? runId = null;
        try
        {
            var machineId = await _machine.GetOrCreateLocalMachineIdAsync(cancellationToken).ConfigureAwait(false);
            runId = await _inventory.StartRunAsync(machineId, AgentMetadata.Version, cancellationToken).ConfigureAwait(false);
            await _inventory.CompleteRunAsync(runId, true, null, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao registar execução de inventário no SQLite.");
            if (runId is not null)
            {
                var msg = ex.Message;
                if (msg.Length > 2000)
                    msg = msg[..2000];
                try
                {
                    await _inventory.CompleteRunAsync(runId, false, msg, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex2)
                {
                    _logger.LogError(ex2, "Falha ao fechar execução de inventário com estado failed.");
                }
            }
        }
    }
}
