using System.Net.NetworkInformation;
using Dapper;

namespace VisionAssets.Persistence;

public sealed class MachineRepository : IMachineRepository
{
    private readonly ISqliteConnectionFactory _connections;

    public MachineRepository(ISqliteConnectionFactory connections)
    {
        _connections = connections;
    }

    public async Task<string> GetOrCreateLocalMachineIdAsync(CancellationToken cancellationToken = default)
    {
        var hostname = Environment.MachineName;
        var domain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
        if (string.IsNullOrWhiteSpace(domain))
            domain = null;

        using var conn = _connections.CreateConnection();
        var existing = await conn.QuerySingleOrDefaultAsync<string>(
            new CommandDefinition(
                "SELECT id FROM machine WHERE hostname = @Hostname LIMIT 1;",
                new { Hostname = hostname },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        var seen = DateTimeOffset.UtcNow.ToString("O");
        if (existing is not null)
        {
            await conn.ExecuteAsync(
                new CommandDefinition(
                    "UPDATE machine SET last_seen_at = @Seen, domain = COALESCE(@Domain, domain) WHERE id = @Id;",
                    new { Seen = seen, Domain = domain, Id = existing },
                    cancellationToken: cancellationToken)).ConfigureAwait(false);
            return existing;
        }

        var id = Guid.NewGuid().ToString("N");
        var os = Environment.OSVersion;
        await conn.ExecuteAsync(
            new CommandDefinition(
                """
                INSERT INTO machine(id, hostname, domain, os_name, os_version, last_seen_at)
                VALUES (@Id, @Hostname, @Domain, @OsName, @OsVersion, @Seen);
                """,
                new
                {
                    Id = id,
                    Hostname = hostname,
                    Domain = domain,
                    OsName = os.Platform.ToString(),
                    OsVersion = os.VersionString,
                    Seen = seen,
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return id;
    }

    public async Task UpdateOperatingSystemAsync(
        string machineId,
        string? osName,
        string? osVersion,
        CancellationToken cancellationToken = default)
    {
        using var conn = _connections.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                "UPDATE machine SET os_name = @Name, os_version = @Ver WHERE id = @Id;",
                new { Name = osName, Ver = osVersion, Id = machineId },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }
}
