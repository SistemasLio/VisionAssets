using Microsoft.Management.Infrastructure;

namespace VisionAssets.Inventory;

internal static class OperatingSystemCollector
{
    public static (string? Caption, string? Version) QueryLocal()
    {
        using var session = CimSession.Create(null);
        foreach (var i in session.QueryInstances(
                     @"root\cimv2",
                     "WQL",
                     "SELECT Caption, Version FROM Win32_OperatingSystem",
                     null))
        {
            using (i)
                return (i.CimInstanceProperties["Caption"]?.Value as string, i.CimInstanceProperties["Version"]?.Value as string);
        }

        return (null, null);
    }
}
