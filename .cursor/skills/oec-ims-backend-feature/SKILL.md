---
name: oec-ims-backend-feature
description: >-
  Step-by-step checklist for adding a MediatR vertical-slice feature in OEC-IMS
  backend. Use when implementing API endpoints, commands, queries, validators,
  or EF persistence for Parts, Orders, Vehicles, Dashboard, or Auth.
---

# OEC-IMS Backend Feature Checklist

## 1. Application slice

Create under `OEC.IMS.Application/Features/{Feature}/`:

| Artifact | Naming |
|----------|--------|
| Command | `{Action}{Entity}Command` |
| Query | `Get{Entity}Query` / `Search{Entities}Query` |
| Handler | `{Name}Handler : IRequestHandler<...>` |
| Validator | `{Command}Validator : AbstractValidator<...>` |
| DTOs | `{Entity}Dto`, `Create{Entity}Request` if needed |
| Mapping | `{Feature}Profile : Profile` |

## 2. Handler rules

- Inject `IApplicationDbContext` (not concrete `DbContext`).
- Commands that mutate multiple tables: explicit transaction.
- Queries: `AsNoTracking()`; prefer `ProjectTo<Dto>()` for lists.
- Throw domain exceptions or let validation fail — no try/catch in handler.

## 3. MediatR pipeline

Register in Application DI:

- `ValidationBehavior` (FluentValidation)
- `LoggingBehavior`

Validators: `AbstractValidator<T>`; async rules for uniqueness checks.

## 4. API endpoint

- Route: `/api/v1/{resource}`
- Controller action: `_mediator.Send(...)` only
- Return: `Ok`, `CreatedAtAction`, `NoContent`, or `ProblemDetails` via middleware

## 5. Infrastructure (if new entity)

- Entity in `Domain/Entities/`
- `IEntityTypeConfiguration<>` in `Infrastructure/Persistence/Configurations/`
- DbSet on `ApplicationDbContext`
- Migration: `dotnet ef migrations add {Name} -p Infrastructure -s Api`

## 6. Tests

- Unit: validator + handler with mocked `IApplicationDbContext`
- Integration: `WebApplicationFactory` + in-memory SQLite for happy path

## Example slice layout

```
Features/Parts/
  Commands/CreatePart/
    CreatePartCommand.cs
    CreatePartCommandHandler.cs
    CreatePartCommandValidator.cs
  Queries/SearchParts/
    SearchPartsQuery.cs
    SearchPartsQueryHandler.cs
  Dtos/PartDto.cs
  Mappings/PartProfile.cs
```
