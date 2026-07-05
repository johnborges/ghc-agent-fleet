# Performance Analysis

## Process
1. **Measure before optimizing** — never optimize based on intuition alone.
2. **Profile in a production-representative environment** — different workloads produce different bottlenecks.
3. **Find the bottleneck** — the 80/20 rule applies; one hotspot usually dominates.
4. **Optimize the hotspot** — do not optimize code that is not on the hot path.
5. **Measure again** — confirm the change produced the expected improvement.
6. **Check for regressions** — run the full benchmark suite, not just the targeted micro-benchmark.

## Profiling Signals
- **CPU bound**: high CPU utilization, flat profile showing one method dominating.
- **Memory bound**: high allocation rate, frequent GC, memory growth over time.
- **I/O bound**: threads blocked on network/disk, high latency with low CPU.
- **Lock contention**: thread starvation, blocked threads in profiler, monitor waits.

## Common Hotspots to Check
- N+1 query patterns — loop issuing individual DB/HTTP calls.
- Unnecessary allocations in tight loops (string concatenation, LINQ on hot paths, boxing).
- Synchronous I/O blocking thread-pool threads (`Result`, `.Wait()` on async code).
- Missing caching for expensive, frequently-called computations with stable inputs.
- Large object heap pressure from oversized buffers retained longer than needed.
- Regex without compiled flag used repeatedly.
- Reflection or dynamic dispatch in hot loops.

## .NET-Specific Guidance
- Use `Span<T>` and `Memory<T>` to reduce heap allocations for slicing and parsing.
- Prefer `ArrayPool<T>.Shared` for short-lived large buffers.
- Use `ValueTask` over `Task` for frequently-completed async operations.
- Profile with `dotnet-trace` + `dotnet-counters`; analyze with PerfView or SpeedScope.
- Check `GC.CollectionCount`, gen0/gen1/gen2 rates, and LOH size under load.

## Reporting
State: (1) the bottleneck identified and how it was measured, (2) the fix applied and why it works, (3) before/after benchmark numbers, (4) any trade-offs introduced (e.g., memory vs. CPU, complexity vs. speed).
