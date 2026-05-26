# Technology Requirements Map

This document maps **every required interview technology** to where it is implemented in OEC-IMS and which **concepts** it demonstrates.

Use this for portfolio reviews, README links, and demo narration.

---

## Requirement checklist

| Required | Package / stack | Implemented | Primary location |
|----------|-----------------|-------------|------------------|
| Yes | **MediatR** | Yes | `backend/src/OEC.IMS.Application/` |
| Yes | **FluentValidation** | Yes | `*Validator.cs`, `ValidationBehavior.cs` |
| Yes | **AutoMapper** | Yes | `Features/*/Mappings/`, handlers |
| Yes | **OpenAPI** | Yes | `OEC.IMS.Api/Program.cs`, Swagger UI |
| Yes | **EF Core** | Yes | `OEC.IMS.Infrastructure/Persistence/` |
| Yes | **Vite** | Yes | `frontend/vite.config.ts` |
| Yes | **React 19** | Yes | `frontend/src/` |
| Yes | **TanStack React Query** | Yes | `frontend/src/features/*/api/` |
| Yes | **Tailwind CSS v4** | Yes | `frontend/src/index.css`, `@theme` |
| Yes | **TypeScript** | Yes | `frontend/tsconfig.json`, all `.ts`/`.tsx` |

**Also demonstrated (supporting, not in your mandatory list):** React Router 7, React Hook Form, Zod, TanStack Table, JWT auth, Serilog, xUnit.

---

## Backend packages

### MediatR — CQRS-lite, pipeline behaviors

**Concepts:** Command/query separation, thin controllers, cross-cutting pipeline (validation, logging).

| Area | Path |
|------|------|
| Registration | `Application/DependencyInjection.cs` — `AddMediatR`, behaviors |
| Pipeline | `Application/Common/Behaviors/ValidationBehavior.cs`, `LoggingBehavior.cs` |
| Commands | `Application/Features/*/Commands/*/*Handler.cs` |
| Queries | `Application/Features/*/Queries/*/*Handler.cs` |
| API dispatch | `Api/Controllers/*Controller.cs` — `Mediator.Send(...)` |

**Example flow:** `POST /api/v1/orders` → `OrdersController` → `CreateOrderCommand` → `CreateOrderCommandHandler` (transaction + stock).

---

### FluentValidation — server-side rules

**Concepts:** Validator per command, async rules (DB), fail before handler runs.

| Area | Path |
|------|------|
| Pipeline hook | `Application/Common/Behaviors/ValidationBehavior.cs` |
| Login | `Features/Auth/Commands/Login/LoginCommandValidator.cs` |
| Parts | `Features/Parts/Commands/CreatePart/CreatePartCommandValidator.cs` (unique SKU async) |
| Orders | `Features/Orders/Commands/CreateOrder/CreateOrderCommandValidator.cs` |
| Stock | `Features/Parts/Commands/AdjustStock/AdjustStockCommandValidator.cs` |
| Unit tests | `tests/OEC.IMS.Application.UnitTests/Validators/CommandValidatorTests.cs` |

**Interview line:** “Client Zod is UX; FluentValidation is authority, especially for stock and SKU uniqueness.”

---

### AutoMapper — entity ↔ DTO mapping

**Concepts:** Feature profiles, `ProjectTo` on `IQueryable` for efficient SQL projections.

| Area | Path |
|------|------|
| Registration | `Application/DependencyInjection.cs` — `AddAutoMapper(assembly)` |
| Profiles | `Features/Parts/Mappings/PartProfile.cs`, `Features/Orders/Mappings/OrderProfile.cs`, `Features/Vehicles/Mappings/VehicleProfile.cs`, etc. |
| ProjectTo lists | `SearchOrdersQueryHandler.cs`, `GetManufacturersQueryHandler.cs`, `GetVehicleModelsQueryHandler.cs` |
| Command results | `CreatePartCommandHandler.cs`, `CreateOrderCommandHandler.cs` — `_mapper.Map<Dto>(entity)` |

**Note:** Some part queries use explicit LINQ `Select` (integration-test stability); mapping pattern is still shown elsewhere.

---

### OpenAPI — API contract & Swagger

**Concepts:** Documented REST contract, JWT security scheme, discoverable endpoints for demos.

| Area | Path |
|------|------|
| Swashbuckle setup | `Api/Program.cs` — `AddSwaggerGen`, Bearer security |
| XML comments | `Api/OEC.IMS.Api.csproj` — `GenerateDocumentationFile` |
| Swagger UI | `/swagger` when running API |
| Codegen stub | `scripts/generate-api-client.ps1` |

**Interview line:** “OpenAPI is the contract between React TypeScript types and C# DTOs.”

---

### EF Core — persistence, migrations, transactions

**Concepts:** Code-first, fluent configurations, global filters, migrations, explicit transactions.

| Area | Path |
|------|------|
| DbContext | `Infrastructure/Persistence/ApplicationDbContext.cs` |
| Interface | `Application/Common/Interfaces/IApplicationDbContext.cs` |
| Configurations | `Infrastructure/Persistence/Configurations/*.cs` |
| Soft delete | `Domain/Common/ISoftDelete.cs`, query filter in context |
| Migrations | `Infrastructure/Persistence/Migrations/` |
| Seeding | `Infrastructure/Persistence/DatabaseSeeder.cs` |
| Transactions | `CreateOrderCommandHandler` via `ExecuteInTransactionAsync` |
| SQLite path | `Infrastructure/Persistence/SqliteConnectionStringResolver.cs` |

**Interview line:** “CreateOrder commits order lines, stock decrement, and StockMovement audit in one transaction.”

---

## Frontend packages

### Vite — build tool & dev server

**Concepts:** Fast HMR, path aliases, env-based API URL, production build for GitHub Pages.

| Area | Path |
|------|------|
| Config | `frontend/vite.config.ts` — `@/` alias, `base` for Pages, dev proxy |
| Env | `VITE_API_URL`, `VITE_BASE_PATH` in deploy workflow |
| Scripts | `package.json` — `dev`, `build` |
| Deploy | `.github/workflows/deploy-frontend-pages.yml` |

---

### React 19 — UI framework

**Concepts:** Feature folders, composition, context (auth), lazy route splitting.

| Area | Path |
|------|------|
| Entry | `frontend/src/main.tsx` |
| App shell | `frontend/src/app/layout/AppShell.tsx` |
| Features | `frontend/src/features/{auth,parts,orders,dashboard,vehicles}/` |
| Lazy routes | `frontend/src/app/routes/AppRouter.tsx` — `React.lazy` + `Suspense` |
| Auth context | `frontend/src/features/auth/context/AuthContext.tsx` |

---

### TanStack React Query — server state

**Concepts:** Query keys, caching, loading/error states, mutation invalidation.

| Area | Path |
|------|------|
| Provider | `frontend/src/app/providers/AppProviders.tsx` |
| Parts | `features/parts/api/use-parts.ts`, `query-keys.ts` |
| Orders | `features/orders/api/use-orders.ts` |
| Dashboard | `features/dashboard/api/use-dashboard.ts` |
| Vehicles | `features/vehicles/api/use-vehicles.ts` |
| Auth login | `features/auth/api/use-login.ts` |
| HTTP client | `frontend/src/shared/api/client.ts` |

**Interview line:** “After create order I invalidate `orders`, `parts`, and `dashboard` keys—no Redux.”

---

### Tailwind CSS v4 — styling & design tokens

**Concepts:** `@theme` CSS variables, utility-first layout, semantic colors (stock, brand).

| Area | Path |
|------|------|
| Theme | `frontend/src/index.css` — `@import 'tailwindcss'`, `@theme` |
| Plugin | `vite.config.ts` — `@tailwindcss/vite` |
| Primitives | `frontend/src/shared/ui/*` — Button, Card, StatCard, Badge |
| Pages | All `features/*/pages/*.tsx` |

---

### TypeScript — type-safe SPA

**Concepts:** Strict typing, shared API types, form inference from Zod.

| Area | Path |
|------|------|
| Config | `frontend/tsconfig.json`, `tsconfig.app.json` |
| API types | `frontend/src/shared/types/api.ts` |
| Zod schemas | `features/parts/schemas/part.schema.ts`, `features/orders/schemas/order.schema.ts` |
| Env types | `frontend/src/vite-env.d.ts` |

---

## Business features vs modules

| Feature | Backend module | Frontend page |
|---------|----------------|---------------|
| Mock JWT login | `Features/Auth` | `LoginPage.tsx` |
| Dashboard KPIs + activity | `Features/Dashboard` | `DashboardPage.tsx` |
| Parts CRUD, search, stock | `Features/Parts` | `PartsPage.tsx` |
| Vehicle fitment | `Features/Vehicles` | `VehiclesPage.tsx` |
| Orders + cancel | `Features/Orders` | `OrdersPage.tsx` |

---

## Architecture concepts covered

| Concept | Where |
|---------|--------|
| Clean Architecture layers | `Domain`, `Application`, `Infrastructure`, `Api` |
| Vertical slices | `Application/Features/{Auth,Parts,...}/` |
| CQRS-lite | Separate Commands vs Queries per feature |
| Repository pattern (via DbContext) | `IApplicationDbContext` |
| DTOs / API contracts | `Application/Features/*/Dtos/` |
| Problem Details errors | API middleware / ASP.NET default |
| Role-based authorization | JWT claims, `[Authorize(Roles = ...)]`, UI `hasRole` |
| Soft delete | Parts — `ISoftDelete` |
| Auditing | `AuditableEntity` |
| Pagination | Parts & orders search endpoints |
| CORS | `Program.cs` — configurable origins |

---

## How to demo (5 minutes)

1. **OpenAPI** — Swagger → login → authorized `GET /parts`.
2. **MediatR + FluentValidation** — Invalid order line → 400 with validation message.
3. **EF Core** — Create order → stock drops on part + dashboard activity.
4. **React Query** — UI shows loading, then refreshed KPIs after mutation.
5. **Tailwind** — Dashboard stat cards, low-stock highlighting on Parts.

---

## Related docs

- [Interview talking points](../interview/talking-points.md)
- [Phase 3 features](../architecture/phase-3-implementation.md)
- [Phase 4 polish](../architecture/phase-4-polish.md)
- [Deployment](../deployment/github-publish.md)
