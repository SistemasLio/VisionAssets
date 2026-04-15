using Dapper;

namespace VisionAssets.Persistence;

public sealed class InventoryDataRepository : IInventoryDataRepository
{
    private readonly ISqliteConnectionFactory _connections;

    public InventoryDataRepository(ISqliteConnectionFactory connections)
    {
        _connections = connections;
    }

    public async Task ReplaceHardwareAsync(
        string machineId,
        IReadOnlyList<HardwareComponentInput> items,
        CancellationToken cancellationToken = default)
    {
        using var conn = _connections.CreateConnection();
        using var tx = conn.BeginTransaction();
        await conn.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM hardware_component WHERE machine_id = @Mid;",
                new { Mid = machineId },
                tx,
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        foreach (var row in items)
        {
            var id = Guid.NewGuid().ToString("N");
            await conn.ExecuteAsync(
                new CommandDefinition(
                    """
                    INSERT INTO hardware_component(id, machine_id, category, manufacturer, model, serial, details_json)
                    VALUES (@Id, @Mid, @Cat, @Mfg, @Model, @Serial, @Details);
                    """,
                    new
                    {
                        Id = id,
                        Mid = machineId,
                        Cat = row.Category,
                        Mfg = row.Manufacturer,
                        Model = row.Model,
                        Serial = row.Serial,
                        Details = row.DetailsJson,
                    },
                    tx,
                    cancellationToken: cancellationToken)).ConfigureAwait(false);
        }

        tx.Commit();
    }

    public async Task ReplaceSoftwareAsync(
        string machineId,
        IReadOnlyList<InstalledSoftwareInput> items,
        CancellationToken cancellationToken = default)
    {
        using var conn = _connections.CreateConnection();
        using var tx = conn.BeginTransaction();
        await conn.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM installed_software WHERE machine_id = @Mid;",
                new { Mid = machineId },
                tx,
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        foreach (var row in items)
        {
            var id = Guid.NewGuid().ToString("N");
            await conn.ExecuteAsync(
                new CommandDefinition(
                    """
                    INSERT INTO installed_software(id, machine_id, name, version, publisher, install_date, source, evidence_json)
                    VALUES (@Id, @Mid, @Name, @Ver, @Pub, @Inst, @Src, @Ev);
                    """,
                    new
                    {
                        Id = id,
                        Mid = machineId,
                        Name = row.Name,
                        Ver = row.Version,
                        Pub = row.Publisher,
                        Inst = row.InstallDate,
                        Src = row.Source,
                        Ev = row.EvidenceJson,
                    },
                    tx,
                    cancellationToken: cancellationToken)).ConfigureAwait(false);
        }

        tx.Commit();
    }
}
