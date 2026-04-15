namespace VisionAssets.Persistence;

/// <summary>Substitui o inventário de hardware/software da máquina por um snapshot completo (por execução).</summary>
public interface IInventoryDataRepository
{
    Task ReplaceHardwareAsync(
        string machineId,
        IReadOnlyList<HardwareComponentInput> items,
        CancellationToken cancellationToken = default);

    Task ReplaceSoftwareAsync(
        string machineId,
        IReadOnlyList<InstalledSoftwareInput> items,
        CancellationToken cancellationToken = default);
}
