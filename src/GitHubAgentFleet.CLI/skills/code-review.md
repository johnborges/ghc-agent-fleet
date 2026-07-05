# Code Review

## Review Priorities (highest → lowest)
1. **Correctness** — Does the code do what it claims? Are there logic errors, off-by-one mistakes, or incorrect assumptions?
2. **Security** — Input validation, auth checks, secrets handling, injection vectors, unsafe deserialization.
3. **Error handling** — Are error paths handled? Are exceptions caught only where recovery is meaningful?
4. **Performance** — N+1 queries, unbounded allocations, synchronous I/O on hot paths.
5. **Readability** — Naming clarity, function length, comment accuracy.
6. **Style** — Consistent with project conventions; minor nits flagged but not blocking.

## Structure of Feedback
- State the file and line range for every finding.
- Classify each: `[blocker]`, `[major]`, `[minor]`, `[nit]`.
- Blockers and majors must include a suggested fix or alternative.
- Nits are optional to address; never require a re-review cycle for nits alone.

## Reviewing a Diff
- Read the PR description and linked issue first to understand intent.
- Trace every public API change to its callsites — verify nothing is silently broken.
- Check test coverage: is the changed behavior covered by new or existing tests?
- Look for missing rollback or cleanup logic (transactions, file handles, connections).

## Common Traps to Check
- `null` references where null is not expected but not guarded.
- Race conditions in async code (shared mutable state, double-checked locking).
- Missing `using` / `Dispose` for IDisposable resources.
- Inconsistent validation: validated in one code path but not another.
- Hardcoded credentials, connection strings, or environment-specific values.

## Tone
Be specific and constructive. Prefer "Consider extracting X because Y" over "This is wrong." Separate your confidence level from the severity: it is fine to ask a question rather than assert an error.
