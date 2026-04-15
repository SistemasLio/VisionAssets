using System.Reflection;

namespace VisionAssets.Agent;

public static class AgentMetadata
{
    public static string Version =>
        typeof(AgentMetadata).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? typeof(AgentMetadata).Assembly.GetName().Version?.ToString()
        ?? "0.0.0";
}
