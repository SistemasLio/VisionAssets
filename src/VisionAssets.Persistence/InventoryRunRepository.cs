using Dapper;

namespace VisionAssets.Persistence;

public sealed class InventoryRunRepository : IInventoryRunRepository
{
    private readonly ISqliteConnectionFactory _connections;

    public InventoryRunRepository(ISqliteConnectionFactory connections)
    {
        _connections = connections;
    }

    public async Task<string> StartRunAsync(string machineId, string agentVersion, CancellationToken cancellationToken = default)
    {
        var id = Guid.NewGuid().ToString("N");
        var started = DateTimeOffset.UtcNow.ToString("O");
        using var conn = _connections.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                """
                INSERT INTO inventory_run(id, machine_id, started_at, finished_at, status, agent_version, error_message)
                VALUES (@Id, @MachineId, @Started, NULL, 'running', @AgentVersion, NULL);
                """,
                new { Id = id, MachineId = machineId, Started = started, AgentVersion = agentVersion },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
        return id;
    }

    public async Task CompleteRunAsync(string runId, bool success, string? errorMessage, CancellationToken cancellationToken = default)
    {
        var finished = DateTimeOffset.UtcNow.ToString("O");
        var status = success ? "success" : "failed";
        using var conn = _connections.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                """
                UPDATE inventory_run
                SET finished_at = @Finished, status = @Status, error_message = @Err
                WHERE id = @Id;
                """,
                new { Id = runId, Finished = finished, Status = status, Err = errorMessage },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }
}
