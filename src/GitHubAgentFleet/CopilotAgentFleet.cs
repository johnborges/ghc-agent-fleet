using GitHub.Copilot;

namespace GitHubAgentFleet;

public static class CopilotAgentFleet
{
    public static IReadOnlyList<CustomAgentConfig> Agents { get; } =
    [
        new CustomAgentConfig
        {
            Name = "tester",
            DisplayName = "Tester",
            Description = "Specialized in validating behavior with targeted tests.",
            Prompt = "Focus on reproducing bugs, writing minimal tests, and confirming fixes.",
            Tools = ["bash", "test-runner"]
        },
        new CustomAgentConfig
        {
            Name = "architect",
            DisplayName = "Architect",
            Description = "Designs maintainable structures and implementation plans.",
            Prompt = "Prioritize clear boundaries, simple abstractions, and incremental evolution.",
            Tools = ["code-search", "dependency-graph"]
        },
        new CustomAgentConfig
        {
            Name = "security",
            DisplayName = "Security",
            Description = "Reviews changes for vulnerabilities and secure defaults.",
            Prompt = "Look for injection, secrets exposure, auth gaps, and unsafe data handling.",
            Tools = ["codeql", "secret-scanner"]
        }
    ];

    public static CustomAgentConfig? FindByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        return Agents.FirstOrDefault(agent =>
            string.Equals(agent.Name, name, StringComparison.OrdinalIgnoreCase));
    }
}
