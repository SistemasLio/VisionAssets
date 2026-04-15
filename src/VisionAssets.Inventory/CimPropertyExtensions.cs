using Microsoft.Management.Infrastructure;

namespace VisionAssets.Inventory;

internal static class CimPropertyExtensions
{
    public static string? GetString(this CimInstance instance, string name) =>
        instance.CimInstanceProperties[name]?.Value as string;

    public static object? GetValue(this CimInstance instance, string name) =>
        instance.CimInstanceProperties[name]?.Value;
}
