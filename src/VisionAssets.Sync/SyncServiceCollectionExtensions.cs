using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace VisionAssets.Sync;

public static class SyncServiceCollectionExtensions
{
    public static IServiceCollection AddVisionAssetsSync(this IServiceCollection services)
    {
        services.AddSingleton<MsalAccessTokenProvider>();
        services.AddHttpClient(InventoryHttpSyncClient.HttpClientName, (sp, client) =>
        {
            var o = sp.GetRequiredService<IOptionsMonitor<BackendOptions>>().CurrentValue;
            client.Timeout = TimeSpan.FromSeconds(Math.Clamp(o.HttpTimeoutSeconds, 10, 600));
        });
        services.AddSingleton<InventoryHttpSyncClient>();
        services.AddSingleton<InventorySyncCoordinator>();
        return services;
    }
}
