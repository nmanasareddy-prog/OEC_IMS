# OEC-IMS Interview Talking Points

Use with [phase-1-architecture-blueprint.md](../architecture/phase-1-architecture-blueprint.md) and `.cursor/skills/oec-ims-interview-map`.

## Elevator pitch (30 seconds)

> OEC-IMS is a dealer-focused parts inventory app built as a modular monolith: React 19 and TanStack Query on the front, .NET 8 with MediatR CQRS-lite, FluentValidation, AutoMapper, and EF Core on the back. I chose SQLite for zero-friction GitHub clones with a documented path to SQL Server. Auth is intentionally mock JWT so I could focus on inventory transactions and API design without IdP setup.

## Technology deep dives

### MediatR

- **What:** IRequest pipeline; one handler per use case.
- **Where:** `Application/Features/*/`, `Application/Common/Behaviors/`.
- **Say:** "Controllers don't contain business logic—they dispatch commands. Validation and logging are pipeline behaviors, which keeps cross-cutting concerns out of handlers."

### FluentValidation

- **What:** `AbstractValidator<T>` per command; runs before handler.
- **Where:** `*Validator.cs` next to commands; `ValidationBehavior`.
- **Say:** "Client-side Zod improves UX, but FluentValidation is the authority—especially for stock checks and order line quantities against the database."

### AutoMapper

- **What:** Profiles per feature; `ProjectTo` for IQueryable lists.
- **Where:** `Features/*/Mappings/`.
- **Say:** "I use ProjectTo on queries to avoid SELECT * and manual mapping in handlers. Small DTOs I might map explicitly to keep profiles obvious."

### EF Core

- **What:** Code-first, configurations, global soft-delete filter, transactions.
- **Where:** `Infrastructure/Persistence/`.
- **Say:** "CreateOrder runs in one transaction: order lines, stock decrement, and stock movement audit rows commit or roll back together."

### OpenAPI

- **What:** Swashbuckle + XML comments; exported contract for frontend.
- **Where:** `Api/Program.cs`, generated types in frontend.
- **Say:** "Contract-first reduces drift between TypeScript and C# DTOs."

### React Query

- **What:** Server state, cache keys, invalidation after mutations.
- **Where:** `frontend/src/features/*/api/`.
- **Say:** "I didn't add Redux—almost all state is server-derived. After creating an order I invalidate parts and dashboard queries."

### Tailwind v4

- **What:** `@theme` CSS variables, shared UI primitives.
- **Where:** `src/index.css`, `shared/ui/`.
- **Say:** "Enterprise density and semantic stock colors without a heavy component library."

## Design decisions to volunteer

| Decision | Why |
|----------|-----|
| Hybrid Clean + vertical slice | Layer discipline without god services |
| int PKs in V1 | Simplicity; GUID path documented |
| No Zustand | React Query owns server cache |
| ProblemDetails not Result wrapper | HTTP-native errors |
| StockMovement table | Audit trail for dashboard and compliance narrative |

## 5-minute live demo script

1. Open Swagger → `POST /auth/login` → copy token.
2. `GET /parts` with pagination and search.
3. `GET /vehicles/.../compatible-parts` (or equivalent).
4. `POST /orders` → show stock change via `GET /parts/{id}` or dashboard.
5. Show React UI equivalent with loading/empty states.

## Questions you might get

**Why not microservices?**  
MVP scope and interview time are better spent on correctness, validation, and transactions inside a modular monolith.

**How would you add real auth?**  
Keep `ICurrentUserService`; swap login for OIDC and map claims—handlers stay unchanged.

**SQLite in production?**  
Not for this demo; ADR-002 documents provider switch and Azure SQL for a real deployment.
