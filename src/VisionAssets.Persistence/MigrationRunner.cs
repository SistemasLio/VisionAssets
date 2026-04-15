using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace VisionAssets.Persistence;

public sealed partial class MigrationRunner : IMigrationRunner
{
    private readonly ISqliteConnectionFactory _connections;
    private readonly ILogger<MigrationRunner> _logger;
    private static readonly Assembly Assembly = typeof(MigrationRunner).Assembly;

    public MigrationRunner(ISqliteConnectionFactory connections, ILogger<MigrationRunner> logger)
    {
        _connections = connections;
        _logger = logger;
    }

    public async Task ApplyPendingAsync(CancellationToken cancellationToken = default)
    {
        using var conn = _connections.CreateConnection();
        await EnsureSchemaMigrationsTableAsync(conn, cancellationToken).ConfigureAwait(false);

        var applied = await LoadAppliedVersionsAsync(conn, cancellationToken).ConfigureAwait(false);
        foreach (var migration in DiscoverMigrations().OrderBy(m => m.Version))
        {
            if (applied.Contains(migration.Version))
                continue;

            _logger.LogInformation("Aplicando migração SQLite {Version} ({Name})", migration.Version, migration.Name);

            using var tx = conn.BeginTransaction();
            try
            {
                foreach (var batch in SplitStatements(migration.Sql))
                {
                    await using var cmd = conn.CreateCommand();
                    cmd.Transaction = tx;
                    cmd.CommandText = batch;
                    await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                }

                await using var ins = conn.CreateCommand();
                ins.Transaction = tx;
                ins.CommandText = "INSERT INTO schema_migrations(version, applied_at) VALUES ($v, $t);";
                ins.Parameters.AddWithValue("$v", migration.Version);
                ins.Parameters.AddWithValue("$t", DateTimeOffset.UtcNow.ToString("O"));
                await ins.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                _logger.LogError(ex, "Falha na migração {Version}", migration.Version);
                throw;
            }
        }
    }

    private static async Task EnsureSchemaMigrationsTableAsync(SqliteConnection conn, CancellationToken cancellationToken)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS schema_migrations (
              version INTEGER NOT NULL PRIMARY KEY,
              applied_at TEXT NOT NULL
            );
            """;
        await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task<HashSet<int>> LoadAppliedVersionsAsync(SqliteConnection conn, CancellationToken cancellationToken)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT version FROM schema_migrations;";
        var set = new HashSet<int>();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            set.Add(reader.GetInt32(0));
        return set;
    }

    private static IEnumerable<MigrationFile> DiscoverMigrations()
    {
        foreach (var fullName in Assembly.GetManifestResourceNames())
        {
            if (!fullName.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                continue;

            var match = VersionRegex().Match(fullName);
            if (!match.Success || !int.TryParse(match.Groups[1].Value, out var version))
                continue;

            using var stream = Assembly.GetManifestResourceStream(fullName);
            if (stream is null)
                continue;
            using var textReader = new StreamReader(stream, Encoding.UTF8);
            var sql = textReader.ReadToEnd();
            yield return new MigrationFile(version, fullName, sql);
        }
    }

    [GeneratedRegex(@"\.(\d{3})_[^.]+\.sql", RegexOptions.CultureInvariant)]
    private static partial Regex VersionRegex();

    /// <summary>Divide script em lotes (comentários de linha -- removidos).</summary>
    internal static IEnumerable<string> SplitStatements(string sql)
    {
        var sb = new StringBuilder();
        foreach (var line in sql.Split('\n'))
        {
            var trim = line.TrimStart();
            if (trim.StartsWith("--", StringComparison.Ordinal))
                continue;
            sb.AppendLine(line);
        }

        var cleaned = sb.ToString();
        foreach (var part in cleaned.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (string.IsNullOrWhiteSpace(part))
                continue;
            yield return part + ";";
        }
    }

    private sealed record MigrationFile(int Version, string Name, string Sql);
}
