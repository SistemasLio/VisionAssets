using Microsoft.Data.Sqlite;

namespace VisionAssets.Persistence;

/// <summary>Abre conexões SQLite com PRAGMAs adequados ao agente.</summary>
public interface ISqliteConnectionFactory
{
    SqliteConnection CreateConnection();
}
