namespace VisionAssets.Persistence;

public interface IInventoryRunRepository
{
    /// <summary>Insere execução com status <c>running</c>.</summary>
    Task<string> StartRunAsync(string machineId, string agentVersion, CancellationToken cancellationToken = default);

    /// <summary>Finaliza execução com <c>success</c> ou <c>failed</c>.</summary>
    Task CompleteRunAsync(string runId, bool success, string? errorMessage, CancellationToken cancellationToken = default);
}
