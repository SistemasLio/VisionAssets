using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace VisionAssets.Sync;

public sealed class MsalAccessTokenProvider
{
    private readonly IOptionsMonitor<BackendOptions> _options;
    private readonly ILogger<MsalAccessTokenProvider> _logger;

    public MsalAccessTokenProvider(IOptionsMonitor<BackendOptions> options, ILogger<MsalAccessTokenProvider> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task<string?> AcquireTokenAsync(CancellationToken cancellationToken = default)
    {
        var o = _options.CurrentValue;
        if (string.IsNullOrWhiteSpace(o.TenantId)
            || string.IsNullOrWhiteSpace(o.ClientId)
            || string.IsNullOrWhiteSpace(o.ClientSecret)
            || string.IsNullOrWhiteSpace(o.ApiScope))
        {
            _logger.LogWarning("Backend: TenantId, ClientId, ClientSecret ou ApiScope em falta; token não obtido.");
            return null;
        }

        try
        {
            var app = ConfidentialClientApplicationBuilder.Create(o.ClientId.Trim())
                .WithClientSecret(o.ClientSecret.Trim())
                .WithTenantId(o.TenantId.Trim())
                .Build();

            var result = await app.AcquireTokenForClient(new[] { o.ApiScope.Trim() })
                .ExecuteAsync(cancellationToken)
                .ConfigureAwait(false);
            return result.AccessToken;
        }
        catch (MsalException ex)
        {
            _logger.LogError(ex, "Falha MSAL ao obter token para o API.");
            return null;
        }
    }
}
