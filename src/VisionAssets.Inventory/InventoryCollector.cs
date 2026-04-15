namespace VisionAssets.Inventory;

/// <summary>Orquestra coleta de hardware + software + metadados de SO.</summary>
public sealed class InventoryCollector
{
    private readonly HardwareCollector _hardware = new();
    private readonly SoftwareCollector _software = new();

    public async Task<InventoryCollectionResult> CollectAsync(
        InventoryCollectionOptions options,
        CancellationToken cancellationToken = default)
    {
        var warnings = new List<string>();
        CollectedHardware[] hw;
        CollectedSoftware[] sw;
        string? caption;
        string? osVer;

        try
        {
            (caption, osVer) = OperatingSystemCollector.QueryLocal();
        }
        catch (Exception ex)
        {
            warnings.Add($"SO: {ex.Message}");
            caption = null;
            osVer = null;
        }

        try
        {
            hw = await _hardware.CollectAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            warnings.Add($"Hardware: {ex.Message}");
            hw = Array.Empty<CollectedHardware>();
        }

        try
        {
            sw = await _software.CollectAsync(options, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            warnings.Add($"Software: {ex.Message}");
            sw = Array.Empty<CollectedSoftware>();
        }

        return new InventoryCollectionResult
        {
            Hardware = hw,
            Software = sw,
            OperatingSystemCaption = caption,
            OperatingSystemVersion = osVer,
            Warnings = warnings,
        };
    }
}
