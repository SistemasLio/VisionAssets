namespace VisionAssets.Inventory;

public sealed record CollectedSoftware(
    string? Name,
    string? Version,
    string? Publisher,
    string? InstallDate,
    string Source);
