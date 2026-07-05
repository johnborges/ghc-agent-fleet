# ghc-agent-fleet

Proof-of-concept and reference implementation for building a multi-agent engineering workflow on top of GitHub Copilot SDK for .NET.

This repository provides:
- A reusable fleet definition with specialized software engineering agents
- A console host that runs interactive Copilot sessions with custom agents, tools, skills, hooks, and MCP servers
- Tests that validate fleet integrity and agent lookup behavior

## Purpose

The goal of this codebase is to demonstrate how to move from a single general-purpose assistant to a role-based fleet model, where each agent is optimized for a specific engineering responsibility (testing, debugging, architecture, code review, security, CI, performance, docs, git workflow, and dependency auditing).

## Current capabilities

### Agent fleet

The fleet currently exposes 12 custom agents:
- tester
- researcher
- architect
- security-auditor
- docs-writer
- code-reviewer
- debugger
- performance-analyzer
- refactorer
- git-assistant
- ci-debugger
- dependency-auditor

Each agent can define:
- A focused prompt and working style
- Allowed tool list
- Optional skill files
- Optional model override

### CLI session host

The console app supports:
- Agent selection by argument or interactive picker
- Streaming assistant and reasoning deltas
- Session memory enabled across turns
- Custom function tool registration (example: get_weather)
- MCP server integration:
	- github via gh mcp-server
	- filesystem via @modelcontextprotocol/server-filesystem
- Session hooks for tool lifecycle and error reporting
- Console-based user elicitation for ask_user style prompts
- Slash commands:
	- /list-agents
	- /help

### Skills integration

Skill markdown files are shipped with the CLI and loaded at runtime through SkillDirectories. The fleet currently includes skill coverage for:
- architecture patterns
- CI debugging
- code review
- debugging
- dependency management
- git workflow
- performance
- refactoring
- security review
- testing strategy

## Tech stack

- .NET 10 (net10.0)
- C# with nullable and implicit usings enabled
- GitHub.Copilot.SDK 1.0.6-preview.1
- xUnit for tests

## Repository layout

- GitHubAgentFleet.slnx
- src/GitHubAgentFleet
	- Fleet definition and agent lookup
- src/GitHubAgentFleet.CLI
	- Interactive console host for running sessions
- tests/GitHubAgentFleet.Tests
	- Unit tests for fleet consistency and lookup semantics

## Prerequisites

- .NET SDK 10.0+
- GitHub Copilot access configured for your environment
- GitHub CLI installed and authenticated (for github MCP server)
- Node.js 18+ and npx (for filesystem MCP server)

## Build and test

```bash
dotnet build
dotnet test
```

## Run

Run with interactive agent selection:

```bash
dotnet run --project src/GitHubAgentFleet.CLI/GitHubAgentFleet.CLI.csproj
```

Run with a specific agent:

```bash
dotnet run --project src/GitHubAgentFleet.CLI/GitHubAgentFleet.CLI.csproj -- --agent debugger
```

Inside the session:
- Type prompts directly
- Use /list-agents to inspect fleet members
- Use /help for command help
- Type exit to quit

## Extending the fleet

1. Add or update an agent in src/GitHubAgentFleet/CopilotAgentFleet.cs.
2. Add any corresponding skill markdown in src/GitHubAgentFleet.CLI/skills.
3. If needed, adjust session behavior in src/GitHubAgentFleet.CLI/Program.cs.
4. Update tests in tests/GitHubAgentFleet.Tests/CopilotAgentFleetTests.cs when introducing or renaming agents.

## Notes

- This is a POC with an intentionally permissive tool-permission policy in the CLI host.
- For production usage, replace approve-all behavior with explicit permission controls and tighter tool/server scopes.
