using System.Text.Json.Serialization;

namespace VisionAssets.Sync;

/// <summary>Corpo JSON alinhado a docs/contracts/inventory-v1.openapi.yaml.</summary>
public sealed class InventorySnapshotDto
{
    [JsonPropertyName("schema_version")]
    public string SchemaVersion { get; init; } = "1.0";

    [JsonPropertyName("machine_id")]
    public required string MachineId { get; init; }

    [JsonPropertyName("inventory_run_id")]
    public required string InventoryRunId { get; init; }

    [JsonPropertyName("collected_at_utc")]
    public DateTimeOffset CollectedAtUtc { get; init; }

    [JsonPropertyName("agent_version")]
    public required string AgentVersion { get; init; }

    [JsonPropertyName("entra_tenant_id")]
    public string? EntraTenantId { get; init; }

    [JsonPropertyName("azure_ad_device_id")]
    public string? AzureAdDeviceId { get; init; }

    [JsonPropertyName("hostname")]
    public string? Hostname { get; init; }

    [JsonPropertyName("operating_system")]
    public OperatingSystemDto? OperatingSystem { get; init; }

    [JsonPropertyName("hardware")]
    public IReadOnlyList<HardwareItemDto> Hardware { get; init; } = Array.Empty<HardwareItemDto>();

    [JsonPropertyName("software")]
    public IReadOnlyList<SoftwareItemDto> Software { get; init; } = Array.Empty<SoftwareItemDto>();
}

public sealed class OperatingSystemDto
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("version")]
    public string? Version { get; init; }
}

public sealed class HardwareItemDto
{
    [JsonPropertyName("category")]
    public required string Category { get; init; }

    [JsonPropertyName("manufacturer")]
    public string? Manufacturer { get; init; }

    [JsonPropertyName("model")]
    public string? Model { get; init; }

    [JsonPropertyName("serial")]
    public string? Serial { get; init; }

    [JsonPropertyName("details_json")]
    public string? DetailsJson { get; init; }
}

public sealed class SoftwareItemDto
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("version")]
    public string? Version { get; init; }

    [JsonPropertyName("publisher")]
    public string? Publisher { get; init; }

    [JsonPropertyName("install_date")]
    public string? InstallDate { get; init; }

    [JsonPropertyName("source")]
    public string? Source { get; init; }

    [JsonPropertyName("evidence_json")]
    public string? EvidenceJson { get; init; }
}
