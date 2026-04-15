using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;

namespace VisionAssets.Sync;

public sealed class InventoryHttpSyncClient
{
    public const string HttpClientName = "VisionAssetsInventory";

    private readonly IHttpClientFactory _httpFactory;
    private readonly IOptionsMonitor<BackendOptions> _options;

    public InventoryHttpSyncClient(IHttpClientFactory httpFactory, IOptionsMonitor<BackendOptions> options)
    {
        _httpFactory = httpFactory;
        _options = options;
    }

    public async Task<HttpResponseMessage> PostSnapshotAsync(
        string jsonBody,
        string idempotencyKey,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        var baseUrl = _options.CurrentValue.BaseUrl?.Trim();
        if (string.IsNullOrEmpty(baseUrl))
            throw new InvalidOperationException("Backend:BaseUrl não configurado.");

        var http = _httpFactory.CreateClient(HttpClientName);
        var root = baseUrl.TrimEnd('/');
        var fullUrl = $"{root}/v1/inventory-snapshots";
        var uri = new Uri(fullUrl, UriKind.Absolute);
        using var req = new HttpRequestMessage(HttpMethod.Post, uri);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        req.Headers.TryAddWithoutValidation("Idempotency-Key", idempotencyKey);
        req.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        return await http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
    }
}
