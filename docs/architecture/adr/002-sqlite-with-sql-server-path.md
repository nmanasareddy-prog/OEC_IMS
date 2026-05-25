# ADR-002: SQLite for V1 with SQL Server Migration Path

## Status

Accepted

## Context

The project must run locally with minimal setup for GitHub recruiters (no SQL Server install). Production-grade portfolios should show awareness of enterprise RDBMS migration.

## Decision

- **V1 runtime default:** SQLite file at `data/oec-ims.db` (gitignored).
- **EF Core** as the single persistence abstraction.
- **Configuration switch** `Database:Provider` = `Sqlite` | `SqlServer` with connection strings in `appsettings` / user secrets.
- **Migrations** authored in `OEC.IMS.Infrastructure` against a provider-agnostic model.

## Consequences

### Positive

- Zero-install clone and run.
- Same codebase demonstrates EF migrations and provider swap in interviews.

### Negative

- Minor provider differences (e.g. certain column types) require discipline and optional SQL Server integration tests later.

## Implementation rules

- Avoid SQLite-only SQL in raw queries.
- Use Fluent API configurations, not provider-specific attributes.
- Document SQL Server connection in README when demoing Azure.

## Alternatives considered

| Alternative | Rejected because |
|-------------|------------------|
| SQL Server only | Friction for open-source clones |
| In-memory only | No migration/portfolio persistence story |

## References

- [Phase 1 Architecture Blueprint](../phase-1-architecture-blueprint.md)
- [database/README.md](../../database/README.md)
