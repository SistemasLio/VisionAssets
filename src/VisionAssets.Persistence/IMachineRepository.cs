namespace VisionAssets.Persistence;

/// <summary>Identidade da máquina local no SQLite (um registo por hostname).</summary>
public interface IMachineRepository
{
    /// <summary>Garante um registo <c>machine</c> para o host atual e devolve o id.</summary>
    Task<string> GetOrCreateLocalMachineIdAsync(CancellationToken cancellationToken = default);

    /// <summary>Atualiza nome/versão do SO a partir da coleta WMI.</summary>
    Task UpdateOperatingSystemAsync(string machineId, string? osName, string? osVersion, CancellationToken cancellationToken = default);
}
