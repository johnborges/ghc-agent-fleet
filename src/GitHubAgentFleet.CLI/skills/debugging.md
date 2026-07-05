# Debugging

## Methodology
1. **Reproduce first** — confirm you can trigger the failure consistently before investigating cause.
2. **Bisect the problem space** — binary search through code paths, commits, or config values to isolate the fault.
3. **Read the error in full** — stack traces, inner exceptions, stderr output. Do not guess from partial information.
4. **Form a hypothesis** — state one specific, falsifiable hypothesis before adding any logging or breakpoints.
5. **Test the hypothesis** — run the minimal change that confirms or refutes it; avoid changing multiple variables at once.
6. **Fix the cause, not the symptom** — suppressing an exception or hardcoding a return value is never a root-cause fix.

## Stack Trace Analysis
- Start from the innermost exception, not the outermost wrapper.
- Identify the first frame in project code (not framework code) — that is the primary investigation point.
- Correlate timestamps from logs with the frame to determine the execution sequence.

## Common Root Causes to Check First
- **Null reference**: uninitialized field, missing null guard, unexpected null from external API.
- **Concurrency**: shared mutable state, missing lock, Task not awaited, CancellationToken ignored.
- **Configuration**: wrong environment variables, missing keys, type mismatch in config binding.
- **State machine**: object used after disposal, method called out of expected sequence.
- **Data shape**: schema mismatch between producer and consumer, encoding issues, truncated input.
- **Off-by-one**: fence-post errors in loops, slice indices, pagination offsets.

## Instrumentation Strategy
- Add structured logging at entry/exit of the suspected function before adding finer-grained probes.
- Use correlation IDs to trace a single request across service boundaries.
- Prefer conditional breakpoints over printf-debugging for latency-sensitive code.

## Reporting a Finding
State: (1) root cause, (2) reproduction steps, (3) why the fix works, (4) what edge cases the fix does not address.
