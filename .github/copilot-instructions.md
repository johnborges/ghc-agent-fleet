# GitHub Copilot (GHC) Agent Fleet — Project Guidelines

## Project Overview
POC showcasing the GitHub Copilot SDK (`GitHub.Copilot.SDK`).
The core library (`GitHubAgentFleet`) exposes a static `CopilotAgentFleet.Agents` list of `CustomAgentConfig` objects.
The CLI (`GitHubAgentFleet.CLI`) demonstrates a `CopilotClient` session.

## Stack
- .NET 10 (`net10.0`), C# with `<Nullable>enable</Nullable>` and `<ImplicitUsings>enable</ImplicitUsings>`
- xUnit for tests
- `GitHub.Copilot.SDK` — `CopilotClient`, `CopilotTool`, `CustomAgentConfig`, `SessionConfig`

## Solution Layout
- `src/GitHubAgentFleet/` — core library (agent definitions, lookup)
- `src/GitHubAgentFleet.CLI/` — console host demonstrating SDK usage
- `tests/GitHubAgentFleet.Tests/` — unit tests

## Build & Test
```bash
dotnet build
dotnet test