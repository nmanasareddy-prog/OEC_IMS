# ADR-001: Hybrid Clean Architecture + Vertical Slice

## Status

Accepted

## Context

OEC-IMS must demonstrate enterprise layering for interviews while remaining maintainable at MVP scope (5 modules, ~10 tables). Pure Clean Architecture risks excessive interfaces; pure vertical slice can blur domain boundaries.

## Decision

Adopt a **hybrid**:

- **Domain** and **Infrastructure** follow Clean Architecture rings.
- **Application** organizes code by **vertical feature slices** (`Features/Parts`, `Features/Orders`, …).
- **Api** remains a thin presentation layer.

CQRS-lite via **MediatR**: separate commands and queries, single database, no event sourcing.

## Consequences

### Positive

- Clear dependency direction for reviews and tests.
- Each use case is one handler — easy to navigate in interviews.
- Cross-cutting validation/logging via MediatR pipelines.

### Negative

- More folders than a single-layer API.
- Team must resist adding repository-per-entity abstractions without need.

## Alternatives considered

| Alternative | Rejected because |
|-------------|------------------|
| Anemic 3-layer (Controller → Service → Repo) | Weak MediatR/CQRS story |
| Microservices | Overkill for portfolio MVP |
| Full CQRS + read models | Unnecessary complexity |

## References

- [Phase 1 Architecture Blueprint](../phase-1-architecture-blueprint.md)
