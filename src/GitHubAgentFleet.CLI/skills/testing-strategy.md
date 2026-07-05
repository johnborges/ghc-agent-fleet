# Testing Strategy

## Core Principles
- Start with the smallest reliable reproduction before writing any test code.
- One assertion per test; multiple assertions per test only when they form a single logical unit.
- Name tests in the format `MethodUnderTest_StateUnderTest_ExpectedBehavior`.
- Test behavior, not implementation — avoid coupling to internal state or private methods.

## Test Pyramid
- **Unit tests** (70%): fast, isolated, no I/O. Mock external dependencies at the boundary.
- **Integration tests** (20%): verify real wiring between components. Keep DB/network scope minimal.
- **End-to-end tests** (10%): cover critical user journeys only. Never gate CI solely on E2E results.

## TDD Workflow
1. Write a failing test that captures the exact expected behavior.
2. Write the minimum production code to make it pass.
3. Refactor under green — never refactor under red.
4. Repeat; commit after each green cycle.

## Mutation Testing
- Run mutation testing (e.g. Stryker) after reaching initial coverage targets.
- A mutation score below 70% indicates tests are asserting the wrong things.
- Target eliminating surviving mutants on arithmetic operators, boundary conditions, and null checks first.

## What to Test
- **Happy paths**: canonical input → expected output.
- **Edge cases**: nulls, empty collections, zero/negative numbers, max values.
- **Error paths**: exceptions thrown, error codes returned, rollback behavior.
- **Contracts**: public API surface behavior that callers depend on.

## What Not to Test
- Private implementation details.
- Framework or third-party library behavior.
- Code that cannot fail (e.g., simple property getters with no logic).

## Reporting
Always state: (1) what the test verifies, (2) what it does NOT cover, (3) which edge cases remain unhandled.
