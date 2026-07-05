namespace GitHubAgentFleet.Tests;

public class CopilotAgentFleetTests
{
    [Fact]
    public void Agents_ShouldExposeRequiredDefaultAgents()
    {
        var names = CopilotAgentFleet.Agents.Select(a => a.Name).ToArray();

        Assert.Equal(new[] { "tester", "architect", "security" }, names);
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
}
