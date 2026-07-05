# Git Workflow

## Commit Messages
Follow the Conventional Commits specification: `<type>(<scope>): <subject>`

Types: `feat`, `fix`, `refactor`, `perf`, `test`, `docs`, `chore`, `ci`, `build`, `revert`

Rules:
- Subject line: imperative mood, ≤72 chars, no period at end.
- Body (optional): explain *why*, not *what*. Wrap at 72 chars.
- Footer: reference issues (`Closes #123`), note breaking changes (`BREAKING CHANGE: ...`).

## Branch Strategy
- `main` / `trunk` — always deployable; direct commits forbidden.
- `feature/<ticket>-<short-desc>` — new features; short-lived.
- `fix/<ticket>-<short-desc>` — bug fixes.
- `release/<semver>` — release preparation; only version bumps and hotfixes allowed.
- `hotfix/<ticket>-<short-desc>` — critical production fixes; merge to main and release branch.

## Rebase vs. Merge
- **Rebase** for feature branches before merging to keep history linear.
- **Merge commit** for integrating long-lived branches (e.g., release → main) to preserve the merge point.
- Never force-push to shared branches.

## Common Workflows
- **Interactive rebase** (`git rebase -i HEAD~N`): squash fixup commits, reorder, reword messages before PR.
- **Cherry-pick** (`git cherry-pick <sha>`): apply a specific commit to another branch without merging history.
- **Bisect** (`git bisect start/bad/good`): binary search for the commit that introduced a regression.
- **Stash** (`git stash push -m "desc"`): save WIP without committing; always use a meaningful description.
- **Worktrees** (`git worktree add`): work on multiple branches simultaneously without stashing.

## Changelog Generation
- Pull from commit history between two tags: `git log v1.2.0..v1.3.0 --pretty=format:"- %s"`.
- Group by type. Remove chore/ci entries from user-facing changelogs.
- Link each entry to its PR or commit SHA.
