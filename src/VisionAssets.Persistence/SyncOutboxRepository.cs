using Dapper;

namespace VisionAssets.Persistence;

public sealed class SyncOutboxRepository : ISyncOutboxRepository
{
    private readonly ISqliteConnectionFactory _connections;

    public SyncOutboxRepository(ISqliteConnectionFactory connections)
    {
        _connections = connections;
    }

    public async Task UpsertPendingAsync(
        string machineId,
        string inventoryRunId,
        string payloadJson,
        CancellationToken cancellationToken = default)
    {
        using var conn = _connections.CreateConnection();
        var existing = await conn.QuerySingleOrDefaultAsync<string>(
            new CommandDefinition(
                "SELECT id FROM sync_outbox WHERE machine_id = @Mid AND inventory_run_id = @Rid LIMIT 1;",
                new { Mid = machineId, Rid = inventoryRunId },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        var created = DateTimeOffset.UtcNow.ToString("O");
        if (existing is not null)
        {
            await conn.ExecuteAsync(
                new CommandDefinition(
                    """
                    UPDATE sync_outbox
                    SET payload_json = @Payload, status = 'pending', last_error = NULL, last_attempt_at = NULL
                    WHERE id = @Id;
                    """,
                    new { Id = existing, Payload = payloadJson },
                    cancellationToken: cancellationToken)).ConfigureAwait(false);
            return;
        }

        var id = Guid.NewGuid().ToString("N");
        await conn.ExecuteAsync(
            new CommandDefinition(
                """
                INSERT INTO sync_outbox(id, machine_id, inventory_run_id, payload_json, status, created_at, attempt_count)
                VALUES (@Id, @Mid, @Rid, @Payload, 'pending', @Created, 0);
                """,
                new { Id = id, Mid = machineId, Rid = inventoryRunId, Payload = payloadJson, Created = created },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<SyncOutboxRow>> ListRetryableAsync(
        int limit,
        int maxAttempts,
        CancellationToken cancellationToken = default)
    {
        using var conn = _connections.CreateConnection();
        var rows = await conn.QueryAsync<SyncOutboxRow>(
            new CommandDefinition(
                """
                SELECT id AS Id, machine_id AS MachineId, inventory_run_id AS InventoryRunId,
                       payload_json AS PayloadJson, status AS Status, attempt_count AS AttemptCount
                FROM sync_outbox
                WHERE status = 'pending' AND attempt_count < @MaxAttempts
                ORDER BY created_at ASC
                LIMIT @Lim;
                """,
                new { Lim = limit, MaxAttempts = maxAttempts },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
        return rows.AsList();
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        using var conn = _connections.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM sync_outbox WHERE id = @Id;",
                new { Id = id },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }

    public async Task RecordFailedAttemptAsync(
        string id,
        string errorMessage,
        int maxAttempts,
        CancellationToken cancellationToken = default)
    {
        var err = errorMessage.Length > 2000 ? errorMessage[..2000] : errorMessage;
        var now = DateTimeOffset.UtcNow.ToString("O");
        using var conn = _connections.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                """
                UPDATE sync_outbox
                SET attempt_count = attempt_count + 1,
                    last_attempt_at = @Now,
                    last_error = @Err,
                    status = CASE WHEN (attempt_count + 1) >= @MaxAttempts THEN 'abandoned' ELSE 'pending' END
                WHERE id = @Id;
                """,
                new { Id = id, Now = now, Err = err, MaxAttempts = maxAttempts },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }
}
