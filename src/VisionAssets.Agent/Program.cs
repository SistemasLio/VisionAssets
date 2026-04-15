using System.Diagnostics;
using Serilog;
using Serilog.Events;
using VisionAssets.Agent;
using VisionAssets.Persistence;
using VisionAssets.Sync;

// Serviço Windows: diretório de trabalho pode ser System32; usar pasta do executável.
var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
});

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "VisionAssets Agent";
});

builder.Services
    .AddOptions<AgentOptions>()
    .Bind(builder.Configuration.GetSection(AgentOptions.SectionName))
    .Validate(o => o.HeartbeatIntervalMinutes is >= 1 and <= 24 * 60, "Agent:HeartbeatIntervalMinutes deve estar entre 1 e 1440.")
    .ValidateOnStart();

builder.Services
    .AddOptions<BackendOptions>()
    .Bind(builder.Configuration.GetSection(BackendOptions.SectionName));

var agentSection = builder.Configuration.GetSection(AgentOptions.SectionName);
var agentOpts = agentSection.Get<AgentOptions>() ?? new AgentOptions();
var logsDir = ResolveLogsDirectory(builder.Environment, agentOpts);
var databasePath = ResolveDatabasePath(builder.Environment, agentOpts);

Directory.CreateDirectory(logsDir);
var logFile = Path.Combine(logsDir, "visionassets-.log");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        logFile,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 14,
        shared: true)
    .CreateLogger();

builder.Services.AddSerilog(Log.Logger, dispose: true);

builder.Services.AddVisionAssetsPersistence(_ =>
{
    var dir = Path.GetDirectoryName(databasePath);
    if (!string.IsNullOrEmpty(dir))
        Directory.CreateDirectory(dir);
    return $"Data Source={databasePath};Cache=Shared";
});

builder.Services.AddVisionAssetsSync();
builder.Services.AddSingleton<InventoryOrchestrator>();
builder.Services.AddHostedService<AgentWorker>();

try
{
    Log.Information(
        "VisionAssets Agent iniciando (PID {Pid}). Logs: {LogsDir}. SQLite: {DbPath}",
        Process.GetCurrentProcess().Id,
        logsDir,
        databasePath);

    var app = builder.Build();
    await app.Services.GetRequiredService<IMigrationRunner>().ApplyPendingAsync().ConfigureAwait(false);
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Falha fatal ao executar o agente.");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync().ConfigureAwait(false);
}

static string ResolveLogsDirectory(IHostEnvironment env, AgentOptions opts)
{
    if (!string.IsNullOrWhiteSpace(opts.LogsDirectory))
        return Path.GetFullPath(opts.LogsDirectory);

    if (env.IsDevelopment())
        return Path.Combine(env.ContentRootPath, "Logs");

    return Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "VisionAssets",
        "Logs");
}

static string ResolveDatabasePath(IHostEnvironment env, AgentOptions opts)
{
    if (!string.IsNullOrWhiteSpace(opts.DatabasePath))
        return Path.GetFullPath(opts.DatabasePath);

    if (env.IsDevelopment())
        return Path.Combine(env.ContentRootPath, "Data", "visionassets.db");

    return Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "VisionAssets",
        "Data",
        "visionassets.db");
}
