using Microsoft.Extensions.DependencyInjection;

namespace VisionAssets.Persistence;

public static class PersistenceServiceCollectionExtensions
{
    /// <summary>Registra SQLite, migrações e repositórios. A fábrica recebe o connection string já resolvido.</summary>
    public static IServiceCollection AddVisionAssetsPersistence(
        this IServiceCollection services,
        Func<IServiceProvider, string> sqliteConnectionStringFactory)
    {
        services.AddSingleton<ISqliteConnectionFactory>(sp =>
            new SqliteConnectionFactory(sqliteConnectionStringFactory(sp)));
        services.AddSingleton<IMigrationRunner, MigrationRunner>();
        services.AddSingleton<IMachineRepository, MachineRepository>();
        services.AddSingleton<IInventoryRunRepository, InventoryRunRepository>();
        return services;
    }
}
