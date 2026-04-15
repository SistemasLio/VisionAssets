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
}
