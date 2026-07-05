# Refactoring

## Rules of Safe Refactoring
1. **Never refactor under a failing test** — all existing tests must be green before starting.
2. **One refactoring at a time** — do not combine behavioral changes with structural changes in the same commit.
3. **Commit after each refactoring step** — small, atomic commits make rollback trivial.
4. **Do not change observable behavior** — if inputs and outputs change, that is not a refactoring, it is a feature change.
5. **Run tests after every step** — not just at the end.

## Common Refactorings

### Extract Method / Function
- When: a block of code can be named with a single intent-revealing phrase.
- How: copy the block, replace with a call, pass inputs as parameters, return outputs.
- Risk: low if the block has clear in/out; higher if it modifies shared state.

### Extract Interface / Abstraction
- When: you need to vary behavior or test in isolation.
- How: define the interface from the caller's perspective (what it needs, not what the class offers).

### Rename Symbol
- When: the name no longer reflects what the symbol does.
- Use IDE rename refactoring — never find-and-replace across files.

### Inline Variable / Method
- When: the indirection adds no clarity and the usage is simple.

### Replace Conditional with Polymorphism
- When: you have multiple type-checking conditionals (`if type == X`) that parallel each other.

### Strangler Fig
- When: incrementally replacing a large legacy component.
- Route traffic to old and new implementations via a facade; shift routing as the new system matures; remove old system last.

## Code Smells to Target First
- Long methods (> 20 lines of logic is a signal, not a rule).
- Deep nesting (> 3 levels of `if`/`for`).
- Duplicate code across more than 2 locations.
- Methods with more than 3–4 parameters (consider a parameter object).
- Comments that explain *what* the code does (extract method so the name explains it instead).

## Reporting
State: (1) the smell targeted, (2) the refactoring applied, (3) what behavior is preserved (cite tests), (4) what further refactoring opportunities remain.
