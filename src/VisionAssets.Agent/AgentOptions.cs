namespace VisionAssets.Agent;

/// <summary>Opções do agente (seção "Agent" no appsettings).</summary>
public sealed class AgentOptions
{
    public const string SectionName = "Agent";

    /// <summary>Intervalo entre heartbeats informativos no log (1–1440 minutos).</summary>
    public int HeartbeatIntervalMinutes { get; set; } = 5;

    /// <summary>
    /// Diretório de logs. Se vazio: em Development usa ./Logs; em produção usa %ProgramData%\VisionAssets\Logs.
    /// </summary>
    public string? LogsDirectory { get; set; }

    /// <summary>
    /// Caminho completo do ficheiro SQLite. Se vazio: Development em ./Data/visionassets.db; produção em %ProgramData%\VisionAssets\Data\visionassets.db.
    /// </summary>
    public string? DatabasePath { get; set; }
}
