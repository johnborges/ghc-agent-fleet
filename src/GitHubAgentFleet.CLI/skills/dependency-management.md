# Dependency Management

## Audit Checklist
- **Version currency**: identify packages more than one major version behind their latest stable release.
- **Known vulnerabilities**: cross-reference against GitHub Advisory Database, NVD, or Snyk. Flag CVEs with CVSS ≥ 7.0 as high priority.
- **Transitive dependencies**: check that direct dependency upgrades do not pull in vulnerable transitive packages.
- **Abandoned packages**: last published > 2 years ago, no open issues addressed, no maintained fork — flag as risk.
- **License compatibility**: identify GPL/AGPL/LGPL in commercial projects; flag copyleft licenses that conflict with the project's distribution model.
- **Duplication**: multiple packages providing the same capability (e.g., two HTTP clients, two JSON serializers) — consolidate.

## Upgrade Strategy
- Upgrade one package at a time and run the full test suite before moving to the next.
- Read the changelog/migration guide for every major version bump.
- For high-CVE packages, create a separate hotfix branch — do not bundle security fixes with feature work.
- Pin transitive dependency versions in lockfiles after a known-good upgrade to prevent surprise updates.

## .NET / NuGet Specifics
- Use `dotnet list package --outdated` for a quick currency report.
- Use `dotnet list package --vulnerable` to surface packages with known CVEs.
- Central Package Management (`Directory.Packages.props`) enforces consistent versions across a solution — recommend adopting if not already in use.
- `<PackageReference>` `PrivateAssets="all"` for dev-only packages (analyzers, test tools) to avoid propagating them to consumers.

## Reporting
State: (1) packages reviewed and count, (2) high-priority findings (CVEs, major version gaps), (3) license issues, (4) recommended upgrade order with risk notes for each.
