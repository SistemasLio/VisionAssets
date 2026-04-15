using Microsoft.Extensions.Options;

namespace VisionAssets.Agent;

/// <summary>Loop principal do agente até existir coleta de inventário (EPIC-003).</summary>
public sealed class AgentWorker : BackgroundService
{
    private readonly ILogger<AgentWorker> _logger;
    private readonly IOptionsMonitor<AgentOptions> _options;

    public AgentWorker(ILogger<AgentWorker> logger, IOptionsMonitor<AgentOptions> options)
    {
        _logger = logger;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("VisionAssets Agent em execução (EPIC-001). Coleta de inventário ainda não configurada.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var minutes = Math.Clamp(_options.CurrentValue.HeartbeatIntervalMinutes, 1, 24 * 60);
            var delay = TimeSpan.FromMinutes(minutes);

            _logger.LogInformation("Heartbeat: agente ativo, próximo em {Interval} minutos.", minutes);

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
}
