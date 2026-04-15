namespace VisionAssets.Persistence;

public sealed record HardwareComponentInput(
    string Category,
    string? Manufacturer,
    string? Model,
    string? Serial,
    string? DetailsJson);

public sealed record InstalledSoftwareInput(
    string? Name,
    string? Version,
    string? Publisher,
    string? InstallDate,
    string? Source,
    string? EvidenceJson);
