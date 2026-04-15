namespace VisionAssets.Inventory;

public sealed record CollectedHardware(
    string Category,
    string? Manufacturer,
    string? Model,
    string? Serial,
    string? DetailsJson);
