# CI/CD Debugging

## Triage Approach
1. **Classify the failure** — build error, test failure, linting/formatting, infrastructure, or flaky test.
2. **Compare to last green run** — check what changed between the last passing and current failing run (commits, config, dependency updates, runner version).
3. **Read the raw log** — expand collapsed log groups; look for the first error line, not just the last.
4. **Reproduce locally** — replicate the CI environment as closely as possible before changing pipeline config.
5. **Fix one thing at a time** — avoid shotgun changes to `.yml` files.

## Common Failure Categories

### Build Failures
- Missing or mismatched SDK/runtime version — check tool setup steps vs. project target framework.
- Missing environment variable or secret not injected into the job context.
- Workspace not clean — artifact from a previous run polluting the build.
- Path case sensitivity — Linux CI fails on path casing that macOS/Windows tolerates.

### Test Failures
- Non-deterministic (flaky) test — retry the job to confirm; then add a quarantine label and file an issue.
- Environment-dependent test — hardcoded localhost port, timezone assumption, missing test database.
- Ordering dependency — test passes alone but fails when run in suite order; check shared static state.
- Timeout — test hangs due to uncancelled async operation or locked resource.

### Dependency/Cache Issues
- Stale cache key — bump cache key version or invalidate manually.
- Registry throttling — add retry logic or use a proxy/mirror.
- Lockfile out of sync — commit the updated lockfile.

## GitHub Actions Specifics
- Use `actions/upload-artifact` to preserve test results, logs, and coverage reports for inspection.
- Enable debug logging: set secret `ACTIONS_STEP_DEBUG=true`.
- Use `continue-on-error: true` + `if: always()` on diagnostic steps to always collect artifacts even on failure.
- Matrix failures: use `fail-fast: false` during investigation to see all combinations.

## Reporting
State: (1) the classification of the failure, (2) the root cause, (3) the fix applied, (4) whether a process change is needed to prevent recurrence.
