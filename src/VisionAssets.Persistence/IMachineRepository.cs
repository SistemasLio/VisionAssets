namespace VisionAssets.Persistence;

/// <summary>Identidade da máquina local no SQLite (um registo por hostname).</summary>
public interface IMachineRepository
{
    /// <summary>Garante um registo <c>machine</c> para o host atual e devolve o id.</summary>
    Task<string> GetOrCreateLocalMachineIdAsync(CancellationToken cancellationToken = default);
}
