# Architecture Patterns

## Core Principles
- **Narrow interfaces** over wide ones — each interface should have one reason to change.
- **Dependency inversion** — high-level modules depend on abstractions; abstractions don't depend on details.
- **Incremental delivery** — prefer small-step changes over big-bang rewrites; every step should leave the system in a deployable state.
- **Call out trade-offs explicitly** — no pattern is universally correct; document what you're trading.
- **Migration risk is a first-class concern** — flag any change that requires coordinated deployment or data migration.

## When to Use Common Patterns

### Layered Architecture
- Use when separating domain logic from infrastructure is the primary goal.
- Domain layer: no dependencies on frameworks, DBs, or HTTP. Pure business logic.
- Application layer: orchestrates use cases; thin.
- Infrastructure layer: persistence, external APIs, messaging.

### Ports and Adapters (Hexagonal)
- Use when you need to swap infrastructure without changing business logic (e.g., different DB, test doubles).
- Define ports as interfaces in the domain; adapters implement them in infrastructure.

### Event-Driven / CQRS
- Use when read and write models have significantly different scaling or complexity requirements.
- Keep events immutable; version them from day one.
- Avoid CQRS for simple CRUD — the overhead is rarely justified.

### Repository Pattern
- Encapsulate data access behind an interface; domain code never references ORM types directly.
- Keep repositories query-focused; avoid leaking EF/SQL-specific constructs into domain code.

## Evaluating a Design
Ask: (1) What changes frequently? (2) What must be independently deployable? (3) Where are the performance boundaries? (4) What are the consistency requirements? Answers to these questions drive which pattern is appropriate.

## Incremental Implementation Plan Template
1. Define the public interface / contract first (no implementation).
2. Write a failing test that exercises the contract.
3. Provide the simplest implementation that passes.
4. Refactor behind the interface with tests green.
5. Wire into the system; keep the old path alive behind a feature flag until validated.
6. Remove the old path once validated.
