using Microsoft.Win32;

namespace VisionAssets.Sync;

/// <summary>Tenta obter o identificador de dispositivo Entra / Azure AD no Windows.</summary>
public static class EntraDeviceIdReader
{
    /// <summary>
    /// Vários caminhos de registo são tentados conforme versão do SO e tipo de junção.
    /// Retorna null se não aplicável ou não disponível.
    /// </summary>
    public static string? TryGetAzureAdDeviceId()
    {
        foreach (var path in RegistryPaths)
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(path.SubKey, writable: false);
                if (key is null)
                    continue;
                var v = key.GetValue(path.ValueName) as string;
                if (!string.IsNullOrWhiteSpace(v))
                    return v.Trim();
            }
            catch
            {
                // ignorar chaves sem permissão ou inexistentes
            }
        }

        return null;
    }

    private static readonly (string SubKey, string ValueName)[] RegistryPaths =
    {
        (@"SOFTWARE\Microsoft\Windows\CurrentVersion\CDJ\AAD", "DeviceId"),
        (@"SYSTEM\CurrentControlSet\Control\CloudDomainJoin\JoinInfo", "DeviceId"),
    };
}
