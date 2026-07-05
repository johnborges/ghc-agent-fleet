using GitHub.Copilot;
using GitHubAgentFleet;
using Microsoft.Extensions.AI;
using System.ComponentModel;

// ── Agent selection ──────────────────────────────────────────────────────────
string? selectedAgent = null;

// Accept --agent <name> as a CLI argument
for (int i = 0; i < args.Length - 1; i++)
{
    if (args[i] is "--agent" or "-a")
    {
        selectedAgent = args[i + 1];
        break;
    }
}

if (selectedAgent is null)
{
    Console.WriteLine("Available agents:");
    Console.WriteLine();
    for (int i = 0; i < CopilotAgentFleet.Agents.Count; i++)
    {
        var a = CopilotAgentFleet.Agents[i];
        Console.WriteLine($"  [{i + 1,2}] {a.Name,-22} {a.Description}");
    }
    Console.WriteLine();
    Console.Write("Select agent (number or name, Enter for default): ");
    var input = Console.ReadLine()?.Trim();

    if (!string.IsNullOrEmpty(input))
    {
        if (int.TryParse(input, out int idx) && idx >= 1 && idx <= CopilotAgentFleet.Agents.Count)
        {
            selectedAgent = CopilotAgentFleet.Agents[idx - 1].Name;
        }
        else
        {
            selectedAgent = CopilotAgentFleet.FindByName(input) is not null ? input : null;
            if (selectedAgent is null)
            {
                Console.WriteLine($"Unknown agent '{input}'. Using default.");
            }
        }
    }
}

var activeAgent = selectedAgent is not null ? CopilotAgentFleet.FindByName(selectedAgent) : null;

// ── Tool definitions ─────────────────────────────────────────────────────────
var getWeather = CopilotTool.DefineTool(
    ([Description("The city name")] string city) =>
    {
        var conditions = new[] { "sunny", "cloudy", "rainy", "partly cloudy" };
        var temp = Random.Shared.Next(50, 80);
        var condition = conditions[Random.Shared.Next(conditions.Length)];
        return new { city, temperature = $"{temp}°F", condition };
    },
    factoryOptions: new AIFunctionFactoryOptions
    {
        Name = "get_weather",
        Description = "Get the current weather for a city",
    });

// ── Session configuration ─────────────────────────────────────────────────────
var skillsDir = Path.Combine(AppContext.BaseDirectory, "skills");
var cwd = Directory.GetCurrentDirectory();

await using var client = new CopilotClient();
await using var session = await client.CreateSessionAsync(new SessionConfig
{
    Model = "auto",
    ReasoningEffort = "medium",
    OnPermissionRequest = PermissionHandler.ApproveAll,
    Streaming = true,

    // Register all fleet agents so the SDK can route to them
    CustomAgents = [.. CopilotAgentFleet.Agents],

    // Activate the selected agent (null = default Copilot agent)
    Agent = selectedAgent,

    // Resolve skill files for all agents that declare Skills
    SkillDirectories = [skillsDir],

    // Persist memory across turns in this session
    Memory = new MemoryConfiguration { Enabled = true },

    // Custom tools available to every agent in the session
    Tools = [getWeather],

    // MCP servers (require gh CLI and Node.js on the host)
    McpServers = new Dictionary<string, McpServerConfig>
    {
        ["github"] = new McpStdioServerConfig
        {
            Command = "gh",
            Args = ["mcp-server"],
        },
        ["filesystem"] = new McpStdioServerConfig
        {
            Command = "npx",
            Args = ["-y", "@modelcontextprotocol/server-filesystem", cwd],
        },
    },

    // Expose ask_user tool — delegate input to the console
    OnUserInputRequest = async (request, _) =>
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"[Input needed] {request.Question}: ");
        Console.ResetColor();
        return new UserInputResponse { Answer = Console.ReadLine() ?? string.Empty, WasFreeform = true };
    },

    // Lifecycle hooks — emit tool and session events to the console
    Hooks = new SessionHooks
    {
        OnSessionStart = async (_, _) => new SessionStartHookOutput(),

        OnPreToolUse = async (input, _) =>
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n  [tool: {input.ToolName}]");
            Console.ResetColor();
            return new PreToolUseHookOutput();
        },

        OnPostToolUse = async (input, _) =>
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("  [done]");
            Console.ResetColor();
            return new PostToolUseHookOutput();
        },

        OnErrorOccurred = async (input, _) =>
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  [error: {input.Error}]");
            Console.ResetColor();
            return new ErrorOccurredHookOutput { ErrorHandling = "skip" };
        },
    },

    // Slash commands available in the session
    Commands =
    [
        new CommandDefinition
        {
            Name = "list-agents",
            Description = "List all available agents in the fleet",
            Handler = async (ctx) =>
            {
                Console.WriteLine();
                Console.WriteLine("Fleet agents:");
                foreach (var a in CopilotAgentFleet.Agents)
                {
                    var marker = string.Equals(a.Name, selectedAgent, StringComparison.OrdinalIgnoreCase) ? " ◀ active" : "";
                    Console.WriteLine($"  {a.Name,-22} {a.Description}{marker}");
                }
            },
        },
        new CommandDefinition
        {
            Name = "help",
            Description = "Show available slash commands",
            Handler = async (ctx) =>
            {
                Console.WriteLine();
                Console.WriteLine("Commands:");
                Console.WriteLine("  /list-agents   List all agents in the fleet");
                Console.WriteLine("  /help          Show this message");
                Console.WriteLine("  exit           Quit the session");
            },
        },
    ],

    DefaultAgent = new DefaultAgentConfig
    {
        ExcludedTools = ["analyze-codebase"],
    },
});

// ── Event listeners ───────────────────────────────────────────────────────────
session.On<SessionEvent>(ev =>
{
    switch (ev)
    {
        case AssistantMessageDeltaEvent delta:
            Console.Write(delta.Data.DeltaContent);
            break;

        case AssistantReasoningDeltaEvent reasoning:
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(reasoning.Data.DeltaContent);
            Console.ResetColor();
            break;

        case SessionIdleEvent:
            Console.WriteLine();
            break;

        case SessionCompactionStartEvent:
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n  [context compaction in progress...]");
            Console.ResetColor();
            break;

        case SessionCompactionCompleteEvent compaction:
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"  [compaction complete — tokens after: {compaction.Data.PostCompactionTokens:N0}]");
            Console.ResetColor();
            break;

        case SessionErrorEvent error:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  [session error: {error.Data.Message}]");
            Console.ResetColor();
            break;
    }
});

// ── Startup banner ────────────────────────────────────────────────────────────
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("╔══════════════════════════════════════════════════╗");
Console.WriteLine("║          GitHub Copilot Agent Fleet              ║");
Console.WriteLine("╚══════════════════════════════════════════════════╝");
Console.ResetColor();

if (activeAgent is not null)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"  Agent  : {activeAgent.DisplayName ?? activeAgent.Name}");
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine($"  {activeAgent.Description}");
    if (activeAgent.Tools?.Count > 0)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  Tools  : {string.Join(", ", activeAgent.Tools)}");
        Console.ResetColor();
    }
    if (activeAgent.Skills?.Count > 0)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  Skills : {string.Join(", ", activeAgent.Skills)}");
        Console.ResetColor();
    }
}
else
{
    Console.WriteLine("  Agent  : Default Copilot");
}

Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("  Type 'exit' to quit. Slash commands: /list-agents  /help");
Console.ResetColor();
Console.WriteLine();

// ── Chat loop ─────────────────────────────────────────────────────────────────
while (true)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("You: ");
    Console.ResetColor();
    var userInput = Console.ReadLine();

    if (string.IsNullOrEmpty(userInput) || userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("Assistant: ");
    Console.ResetColor();
    await session.SendAndWaitAsync(new MessageOptions { Prompt = userInput });
    Console.WriteLine();
}
