namespace VisionAssets.Persistence;

public sealed record SyncOutboxRow(
    string Id,
    string MachineId,
    string InventoryRunId,
    string PayloadJson,
    string Status,
    int AttemptCount);
