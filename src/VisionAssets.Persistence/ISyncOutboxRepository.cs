namespace VisionAssets.Persistence;

public interface ISyncOutboxRepository
{
    /// <summary>Insere ou atualiza payload para o par (máquina, execução).</summary>
    Task UpsertPendingAsync(
        string machineId,
        string inventoryRunId,
        string payloadJson,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SyncOutboxRow>> ListRetryableAsync(int limit, int maxAttempts, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);

    Task RecordFailedAttemptAsync(string id, string errorMessage, int maxAttempts, CancellationToken cancellationToken = default);
}
