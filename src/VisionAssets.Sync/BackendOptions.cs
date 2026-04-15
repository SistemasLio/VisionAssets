namespace VisionAssets.Sync;

/// <summary>Configuração do API central e Entra ID (secção "Backend").</summary>
public sealed class BackendOptions
{
    public const string SectionName = "Backend";

    /// <summary>Quando false, não há chamadas HTTP nem fila (por defeito).</summary>
    public bool Enabled { get; set; }

    /// <summary>URL base HTTPS (ex.: https://api.contoso.com).</summary>
    public string? BaseUrl { get; set; }

    /// <summary>ID do tenant Entra ID.</summary>
    public string? TenantId { get; set; }

    /// <summary>Application (client) ID do agente.</summary>
    public string? ClientId { get; set; }

    /// <summary>Client secret (preferir User Secrets ou cofre em produção).</summary>
    public string? ClientSecret { get; set; }

    /// <summary>Scope da API (ex.: api://xxx/.default).</summary>
    public string? ApiScope { get; set; }

    public int HttpTimeoutSeconds { get; set; } = 120;

    /// <summary>Máximo de tentativas de envio por entrada na outbox.</summary>
    public int OutboxMaxAttempts { get; set; } = 10;

    /// <summary>Lotes processados por ciclo de retry.</summary>
    public int OutboxBatchSize { get; set; } = 5;
}
