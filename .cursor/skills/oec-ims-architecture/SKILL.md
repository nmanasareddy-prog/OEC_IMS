---
name: oec-ims-architecture
description: >-
  Enforces OEC-IMS Clean Architecture layer rules and dependency direction.
  Use when adding projects, moving code between layers, reviewing structure,
  or implementing any OEC Inventory Management System feature.
---

# OEC-IMS Architecture Rules

Reference: [docs/architecture/phase-1-architecture-blueprint.md](../../docs/architecture/phase-1-architecture-blueprint.md)

## Layer dependency (strict)

```
Api → Application → Domain
Infrastructure → Application, Domain
```

| Layer | Contains | Must NOT reference |
|-------|----------|------------------|
| **Domain** | Entities, enums, domain exceptions | EF, MediatR, ASP.NET, DTOs |
| **Application** | Commands, queries, handlers, validators, DTOs, mappings | EF concrete types, `DbContext` |
| **Infrastructure** | EF Core, configurations, migrations, `IApplicationDbContext` impl | Api |
| **Api** | Controllers, middleware, DI registration | Business logic in controllers |

## Patterns

- **Hybrid:** Clean rings + vertical slices under `Application/Features/{FeatureName}/`.
- **CQRS-lite:** One MediatR handler per use case; no event sourcing.
- **Controllers:** Thin — only `IMediator.Send` + return mapped HTTP result.
- **No** generic repository per entity; use `IApplicationDbContext` in handlers.
- **No** `ApiResponse<T>` success wrapper; use HTTP status + ProblemDetails for errors.

## V1 modules

Auth (mock), Dashboard, Parts, Vehicles (compatibility), Orders.

## Defaults

- PKs: `int` identity
- DB: SQLite; SQL Server via config later
- API: `/api/v1/...`

## Before merging

- [ ] Domain has zero infrastructure references
- [ ] New feature has Application folder slice, not a new "Services" god class
- [ ] Validators in Application, wired via MediatR pipeline
