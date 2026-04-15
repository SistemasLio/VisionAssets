namespace VisionAssets.Persistence;

public interface IMigrationRunner
{
    /// <summary>Aplica migrações pendentes em ordem (transação por migração).</summary>
    Task ApplyPendingAsync(CancellationToken cancellationToken = default);
}
