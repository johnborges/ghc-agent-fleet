using GitHubAgentFleet;

foreach (var agent in CopilotAgentFleet.Agents)
{
    Console.WriteLine($"{agent.DisplayName} ({agent.Name}): {agent.Description}");
}
