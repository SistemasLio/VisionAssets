using System.Text.Json;
using Microsoft.Extensions.Options;
using VisionAssets.Inventory;
using VisionAssets.Persistence;

namespace VisionAssets.Agent;

/// <summary>Executa coleta WMI/Registry e grava snapshot no SQLite.</summary>
public sealed class InventoryOrchestrator
{
    private readonly InventoryCollector _collector = new();
    private readonly IMachineRepository _machine;
    private readonly IInventoryDataRepository _data;
    private readonly IOptionsMonitor<AgentOptions> _options;
    private readonly ILogger<InventoryOrchestrator> _logger;

    public InventoryOrchestrator(
        IMachineRepository machine,
        IInventoryDataRepository data,
        IOptionsMonitor<AgentOptions> options,
        ILogger<InventoryOrchestrator> logger)
    {
        _machine = machine;
        _data = data;
        _options = options;
        _logger = logger;
    }

    public async Task<InventoryCollectionResult> RunAsync(string machineId, CancellationToken cancellationToken = default)
    {
        var o = _options.CurrentValue;
        var invOptions = new InventoryCollectionOptions
        {
            IncludeCurrentUserUninstallKeys = o.IncludeCurrentUserUninstallKeys,
            CimQueryTimeoutMs = o.CimQueryTimeoutMs,
        };

        var result = await _collector.CollectAsync(invOptions, cancellationToken).ConfigureAwait(false);

        await _machine
            .UpdateOperatingSystemAsync(machineId, result.OperatingSystemCaption, result.OperatingSystemVersion, cancellationToken)
            .ConfigureAwait(false);

        var hw = result.Hardware
            .Select(
                h => new HardwareComponentInput(
                    h.Category,
                    h.Manufacturer,
                    h.Model,
                    h.Serial,
                    h.DetailsJson))
            .ToList();

        var sw = result.Software
            .Select(
                s => new InstalledSoftwareInput(
                    s.Name,
                    s.Version,
                    s.Publisher,
                    s.InstallDate,
                    s.Source,
                    JsonSerializer.Serialize(new { source = s.Source })))
            .ToList();

        await _data.ReplaceHardwareAsync(machineId, hw, cancellationToken).ConfigureAwait(false);
        await _data.ReplaceSoftwareAsync(machineId, sw, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation(
            "Inventário gravado: {Hw} linhas de hardware, {Sw} aplicações.",
            hw.Count,
            sw.Count);

        foreach (var w in result.Warnings)
            _logger.LogWarning("Coleta com aviso: {Warning}", w);

        return result;
    }
}
