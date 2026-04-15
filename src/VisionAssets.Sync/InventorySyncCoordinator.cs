using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VisionAssets.Inventory;
using VisionAssets.Persistence;

namespace VisionAssets.Sync;

/// <summary>Envia snapshot ao API central; em falta persiste na outbox SQLite.</summary>
public sealed class InventorySyncCoordinator
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private readonly IOptionsMonitor<BackendOptions> _backendOptions;
    private readonly MsalAccessTokenProvider _tokens;
    private readonly InventoryHttpSyncClient _http;
    private readonly ISyncOutboxRepository _outbox;
    private readonly ILogger<InventorySyncCoordinator> _logger;

    public InventorySyncCoordinator(
        IOptionsMonitor<BackendOptions> backendOptions,
        MsalAccessTokenProvider tokens,
        InventoryHttpSyncClient http,
        ISyncOutboxRepository outbox,
        ILogger<InventorySyncCoordinator> logger)
    {
        _backendOptions = backendOptions;
        _tokens = tokens;
        _http = http;
        _outbox = outbox;
        _logger = logger;
    }

    public async Task TrySendAfterInventoryAsync(
        string machineId,
        string inventoryRunId,
        string agentVersion,
        InventoryCollectionResult result,
        CancellationToken cancellationToken = default)
    {
        var o = _backendOptions.CurrentValue;
        if (!o.Enabled)
            return;

        var json = BuildPayloadJson(machineId, inventoryRunId, agentVersion, result, o.TenantId);
        await TrySendOrEnqueueAsync(machineId, inventoryRunId, json, o, cancellationToken).ConfigureAwait(false);
    }

    public async Task ProcessOutboxAsync(CancellationToken cancellationToken = default)
    {
        var o = _backendOptions.CurrentValue;
        if (!o.Enabled)
            return;

        var batch = Math.Clamp(o.OutboxBatchSize, 1, 100);
        var maxAttempts = Math.Max(1, o.OutboxMaxAttempts);
        var pending = await _outbox.ListRetryableAsync(batch, maxAttempts, cancellationToken).ConfigureAwait(false);
        foreach (var row in pending)
        {
            await TrySendOutboxRowAsync(row, o, maxAttempts, cancellationToken).ConfigureAwait(false);
        }
    }

    private string BuildPayloadJson(
        string machineId,
        string inventoryRunId,
        string agentVersion,
        InventoryCollectionResult result,
        string? tenantId)
    {
        var hostname = Environment.MachineName;
        var azureDevice = EntraDeviceIdReader.TryGetAzureAdDeviceId();
        var dto = InventorySnapshotBuilder.Build(
            machineId,
            inventoryRunId,
            agentVersion,
            hostname,
            tenantId,
            azureDevice,
            result);
        return JsonSerializer.Serialize(dto, JsonOptions);
    }

    private async Task TrySendOrEnqueueAsync(
        string machineId,
        string inventoryRunId,
        string json,
        BackendOptions o,
        CancellationToken cancellationToken)
    {
        var maxAttempts = Math.Max(1, o.OutboxMaxAttempts);
        var token = await _tokens.AcquireTokenAsync(cancellationToken).ConfigureAwait(false);
        if (token is null)
        {
            await _outbox.UpsertPendingAsync(machineId, inventoryRunId, json, cancellationToken).ConfigureAwait(false);
            _logger.LogWarning("Snapshot para run {Run} guardado na outbox (token indisponível).", inventoryRunId);
            return;
        }

        try
        {
            var idem = $"{machineId}:{inventoryRunId}";
            using var resp = await _http.PostSnapshotAsync(json, idem, token, cancellationToken).ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                _logger.LogInformation("Snapshot enviado ao API (run {Run}).", inventoryRunId);
                return;
            }

            var body = await SafeReadContentAsync(resp, cancellationToken).ConfigureAwait(false);
            var msg = $"HTTP {(int)resp.StatusCode}: {body}";
            await _outbox.UpsertPendingAsync(machineId, inventoryRunId, json, cancellationToken).ConfigureAwait(false);
            _logger.LogWarning("Envio rejeitado; snapshot na outbox (run {Run}): {Msg}", inventoryRunId, msg);
        }
        catch (Exception ex)
        {
            await _outbox.UpsertPendingAsync(machineId, inventoryRunId, json, cancellationToken).ConfigureAwait(false);
            _logger.LogWarning(ex, "Falha de rede ao enviar snapshot; run {Run} na outbox.", inventoryRunId);
        }
    }

    private async Task TrySendOutboxRowAsync(
        SyncOutboxRow row,
        BackendOptions o,
        int maxAttempts,
        CancellationToken cancellationToken)
    {
        var token = await _tokens.AcquireTokenAsync(cancellationToken).ConfigureAwait(false);
        if (token is null)
            return;

        try
        {
            var idem = $"{row.MachineId}:{row.InventoryRunId}";
            using var resp = await _http.PostSnapshotAsync(row.PayloadJson, idem, token, cancellationToken).ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                await _outbox.DeleteAsync(row.Id, cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Outbox enviada (id {Id}).", row.Id);
                return;
            }

            var body = await SafeReadContentAsync(resp, cancellationToken).ConfigureAwait(false);
            var msg = $"HTTP {(int)resp.StatusCode}: {body}";
            await _outbox.RecordFailedAttemptAsync(row.Id, msg, maxAttempts, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await _outbox.RecordFailedAttemptAsync(row.Id, ex.Message, maxAttempts, cancellationToken).ConfigureAwait(false);
        }
    }

    private static async Task<string> SafeReadContentAsync(HttpResponseMessage resp, CancellationToken cancellationToken)
    {
        try
        {
            return await resp.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            return string.Empty;
        }
    }
}
