---
name: oec-ims-interview-map
description: >-
  Maps OEC-IMS interview technologies to concrete code locations and talking
  points. Use when preparing demos, README interview sections, or explaining
  where MediatR, FluentValidation, EF Core, React Query, etc. are demonstrated.
---

# OEC-IMS Interview Technology Map

Full narrative: [docs/interview/talking-points.md](../../docs/interview/talking-points.md)  
**Requirements map (all packages + file paths):** [docs/requirements/technology-requirements-map.md](../../docs/requirements/technology-requirements-map.md)

## Backend

| Technology | Where to look | Talking point |
|------------|---------------|---------------|
| **MediatR** | `Application/Common/Behaviors/`, `Features/*/Handlers/` | Pipeline behaviors; thin controllers |
| **FluentValidation** | `Features/*/Validators/`, `ValidationBehavior` | Server authority; async SKU check |
| **AutoMapper** | `Features/*/Mappings/`, `ProjectTo` in queries | DTO projection vs anemic entities |
| **EF Core** | `Infrastructure/Persistence/`, migrations | Configurations, soft delete filter, transactions on CreateOrder |
| **OpenAPI** | `Api/Program.cs`, Swashbuckle, XML docs | Contract-first; codegen for frontend |

## Frontend

| Technology | Where to look | Talking point |
|------------|---------------|---------------|
| **React 19** | `features/*/pages/`, `app/` | Feature slices; lazy routes |
| **Vite** | `vite.config.ts`, env `VITE_API_URL` | Fast dev; static deploy |
| **TanStack Query** | `features/*/api/` | Cache, invalidation, mutation loading |
| **Tailwind v4** | `src/index.css` `@theme`, `shared/ui/` | Design tokens; dark mode |
| **TypeScript** | `shared/api/generated/` | Strict; types from OpenAPI |

## Demo script (5 min)

1. Mock login → JWT in SPA
2. Search parts → paginated table
3. Vehicle compatibility → parts for a year/model
4. Create order → stock deduct + dashboard refresh
5. Swagger → show validator + handler flow

## Anti-patterns to call out (you avoided)

- God `PartService` instead of handlers
- Redux duplicating React Query cache
- Generic `ApiResponse<T>` on every endpoint
