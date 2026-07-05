namespace GitHubAgentFleet.Tests;

public class CopilotAgentFleetTests
{
    private static readonly string[] ExpectedAgentNames =
    [
        "tester", "researcher", "architect", "security-auditor", "docs-writer",
        "code-reviewer", "debugger", "performance-analyzer", "refactorer",
        "git-assistant", "ci-debugger", "dependency-auditor"
    ];

    [Fact]
    public void Agents_ShouldExposeRequiredDefaultAgents()
    {
        var names = CopilotAgentFleet.Agents.Select(a => a.Name).ToArray();

        Assert.Equal(ExpectedAgentNames, names);
    }

    [Theory]
    [InlineData("tester")]
    [InlineData("Tester")]
    [InlineData("TESTER")]
    public void FindByName_ShouldBeCaseInsensitive(string input)
    {
        var agent = CopilotAgentFleet.FindByName(input);

        Assert.NotNull(agent);
        Assert.Equal("tester", agent!.Name);
    }

    [Fact]
    public void FindByName_ShouldReturnNull_ForUnknownOrBlankName()
    {
        Assert.Null(CopilotAgentFleet.FindByName("unknown"));
        Assert.Null(CopilotAgentFleet.FindByName(""));
        Assert.Null(CopilotAgentFleet.FindByName("   "));
    }

    [Theory]
    [InlineData("code-reviewer")]
    [InlineData("debugger")]
    [InlineData("performance-analyzer")]
    [InlineData("refactorer")]
    [InlineData("git-assistant")]
    [InlineData("ci-debugger")]
    [InlineData("dependency-auditor")]
    public void FindByName_ShouldResolveAllNewAgents(string name)
    {
        var agent = CopilotAgentFleet.FindByName(name);

        Assert.NotNull(agent);
        Assert.Equal(name, agent!.Name);
    }

    [Fact]
    public void AllAgents_ShouldHaveNonEmptyRequiredProperties()
    {
        for (int i = 0; i < CopilotAgentFleet.Agents.Count; i++)
        {
            var agent = CopilotAgentFleet.Agents[i];
            Assert.False(string.IsNullOrWhiteSpace(agent.Name),
                $"Agent at index {i} has no Name.");
            Assert.False(string.IsNullOrWhiteSpace(agent.Description),
                $"Agent '{agent.Name}' has no Description.");
            Assert.False(string.IsNullOrWhiteSpace(agent.Prompt),
                $"Agent '{agent.Name}' has no Prompt.");
        }
    }

    [Fact]
    public void AllAgents_NamesShouldBeUnique()
    {
        var names = CopilotAgentFleet.Agents.Select(a => a.Name).ToList();
        var distinct = names.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        Assert.Equal(distinct.Count, names.Count);
    }

    [Theory]
    [InlineData("tester")]
    [InlineData("architect")]
    [InlineData("security-auditor")]
    [InlineData("code-reviewer")]
    [InlineData("debugger")]
    [InlineData("performance-analyzer")]
    [InlineData("refactorer")]
    [InlineData("git-assistant")]
    [InlineData("ci-debugger")]
    [InlineData("dependency-auditor")]
    public void SkillCarryingAgents_ShouldHaveSkillsDefined(string name)
    {
        var agent = CopilotAgentFleet.FindByName(name);

        Assert.NotNull(agent);
        Assert.NotNull(agent!.Skills);
        Assert.NotEmpty(agent.Skills!);
    }
}
