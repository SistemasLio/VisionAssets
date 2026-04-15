namespace VisionAssets.Inventory;

public sealed class InventoryCollectionOptions
{
    /// <summary>Incluir HKCU\...\Uninstall (pode refletir só o utilizador atual do serviço).</summary>
    public bool IncludeCurrentUserUninstallKeys { get; set; }

    /// <summary>Timeout aplicado a cada consulta CIM (ms).</summary>
    public int CimQueryTimeoutMs { get; set; } = 60_000;
}
