using System.Diagnostics;
using Serilog;
using Serilog.Events;
using VisionAssets.Agent;

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

var agentSection = builder.Configuration.GetSection(AgentOptions.SectionName);
var agentOpts = agentSection.Get<AgentOptions>() ?? new AgentOptions();
var logsDir = ResolveLogsDirectory(builder.Environment, agentOpts);

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

builder.Services.AddHostedService<AgentWorker>();

try
{
    Log.Information("VisionAssets Agent iniciando (PID {Pid}). Logs em {LogsDir}", Process.GetCurrentProcess().Id, logsDir);
    await builder.Build().RunAsync().ConfigureAwait(false);
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
