using Microsoft.Win32;

namespace VisionAssets.Inventory;

/// <summary>Lista software a partir das chaves Uninstall do Registry (sem Win32_Product).</summary>
public sealed class SoftwareCollector
{
    private const string UninstallRelative = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

    public Task<CollectedSoftware[]> CollectAsync(InventoryCollectionOptions options, CancellationToken cancellationToken = default) =>
        Task.Run(
            () =>
            {
                var dedup = new Dictionary<string, CollectedSoftware>(StringComparer.OrdinalIgnoreCase);

                using (var k64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(UninstallRelative))
                    EnumerateUninstallKey(k64, @"HKLM\Uninstall", dedup);

                using (var k32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(UninstallRelative))
                    EnumerateUninstallKey(k32, @"HKLM\WOW6432Node\Uninstall", dedup);

                if (options.IncludeCurrentUserUninstallKeys)
                {
                    using var cu = Registry.CurrentUser.OpenSubKey(UninstallRelative);
                    EnumerateUninstallKey(cu, @"HKCU\Uninstall", dedup);
                }

                return dedup.Values.ToArray();
            },
            cancellationToken);

    private static void EnumerateUninstallKey(
        RegistryKey? uninstall,
        string sourceLabel,
        Dictionary<string, CollectedSoftware> dedup)
    {
        if (uninstall is null)
            return;

        foreach (var name in uninstall.GetSubKeyNames())
        {
            using var key = uninstall.OpenSubKey(name);
            if (key is null)
                continue;

            var displayName = key.GetValue("DisplayName") as string;
            if (string.IsNullOrWhiteSpace(displayName))
                continue;

            var version = key.GetValue("DisplayVersion") as string;
            var publisher = key.GetValue("Publisher") as string;
            var installDate = key.GetValue("InstallDate") as string;
            if (key.GetValue("SystemComponent") is int sc && sc == 1)
                continue;

            var row = new CollectedSoftware(
                displayName.Trim(),
                string.IsNullOrWhiteSpace(version) ? null : version.Trim(),
                string.IsNullOrWhiteSpace(publisher) ? null : publisher.Trim(),
                NormalizeInstallDate(installDate),
                $"{sourceLabel}\\{name}");

            var keyDedup = $"{row.Name}\u001f{row.Version}\u001f{row.Publisher}";
            dedup[keyDedup] = row;
        }
    }

    private static string? NormalizeInstallDate(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return null;
        var t = raw.Trim();
        return t.Length == 8 && t.All(char.IsDigit) ? t : t;
    }
}
