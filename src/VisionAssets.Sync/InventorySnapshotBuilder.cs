using System.Text.Json;
using VisionAssets.Inventory;

namespace VisionAssets.Sync;

public static class InventorySnapshotBuilder
{
    public static InventorySnapshotDto Build(
        string machineId,
        string inventoryRunId,
        string agentVersion,
        string hostname,
        string? entraTenantId,
        string? azureAdDeviceId,
        InventoryCollectionResult result)
    {
        var hw = result.Hardware.Select(
                h => new HardwareItemDto
                {
                    Category = h.Category,
                    Manufacturer = h.Manufacturer,
                    Model = h.Model,
                    Serial = h.Serial,
                    DetailsJson = h.DetailsJson,
                })
            .ToList();

        var sw = result.Software.Select(
                s => new SoftwareItemDto
                {
                    Name = s.Name,
                    Version = s.Version,
                    Publisher = s.Publisher,
                    InstallDate = s.InstallDate,
                    Source = s.Source,
                    EvidenceJson = JsonSerializer.Serialize(new { source = s.Source }),
                })
            .ToList();

        return new InventorySnapshotDto
        {
            MachineId = machineId,
            InventoryRunId = inventoryRunId,
            CollectedAtUtc = DateTimeOffset.UtcNow,
            AgentVersion = agentVersion,
            EntraTenantId = entraTenantId,
            AzureAdDeviceId = azureAdDeviceId,
            Hostname = hostname,
            OperatingSystem = new OperatingSystemDto
            {
                Name = result.OperatingSystemCaption,
                Version = result.OperatingSystemVersion,
            },
            Hardware = hw,
            Software = sw,
        };
    }
}
