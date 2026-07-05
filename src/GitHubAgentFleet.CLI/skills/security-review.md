# Security Review

## Threat Modeling First
Before looking at code, identify: (1) what assets are being protected, (2) who the actors are (authenticated user, anonymous user, internal service, external API), (3) what trust boundaries exist in the data flow.

## OWASP Top 10 Checklist
- **A01 Broken Access Control** — Verify authorization checks on every endpoint and resource. Check for IDOR (insecure direct object reference) by comparing request parameters to session identity.
- **A02 Cryptographic Failures** — No sensitive data in logs, URLs, or cookies. TLS enforced. Correct algorithms (AES-256, SHA-256+; not MD5/SHA1 for security). Secrets not hardcoded.
- **A03 Injection** — Parameterized queries for all SQL. Validated/escaped output for HTML. Sanitized inputs for OS commands. Avoid `eval`, `exec`, dynamic query construction.
- **A04 Insecure Design** — Rate limiting on auth endpoints. Account lockout after repeated failures. Sensitive operations require re-authentication.
- **A05 Security Misconfiguration** — CORS not wildcard on credentialed endpoints. Debug endpoints disabled in production. Verbose error messages not exposed to clients.
- **A06 Vulnerable Components** — Check dependency versions against known CVE databases. Flag transitive dependencies.
- **A07 Auth/Session Failures** — Short-lived tokens. Secure, HttpOnly, SameSite cookies. Server-side session invalidation on logout.
- **A08 Software/Data Integrity** — Verify signatures of external artifacts. Integrity checks on deserialized data.
- **A09 Logging/Monitoring** — Security events logged (failed auth, access denied, input validation failure). PII not logged.
- **A10 SSRF** — Validate and allowlist outbound request targets. Block internal/metadata endpoints.

## Findings Format
Each finding must include: **severity** (Critical/High/Medium/Low/Informational), **location** (file:line), **description**, **impact**, **recommendation**, and **confidence level**.

## Distinguish Verified vs. Assumed
Always state whether a finding is confirmed (exploitable with a proof-of-concept) or assumed (requires further context to confirm).
