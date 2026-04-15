namespace VisionAssets.Inventory;

public sealed class InventoryCollectionResult
{
    public IReadOnlyList<CollectedHardware> Hardware { get; init; } = Array.Empty<CollectedHardware>();

    public IReadOnlyList<CollectedSoftware> Software { get; init; } = Array.Empty<CollectedSoftware>();

    public string? OperatingSystemCaption { get; init; }

    public string? OperatingSystemVersion { get; init; }

    public IReadOnlyList<string> Warnings { get; init; } = Array.Empty<string>();
}
