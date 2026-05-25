# OEC Inventory Management System (OEC-IMS)

Enterprise-grade automotive **Inventory & Parts Management** — portfolio and interview showcase.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-19-61DAFB)](https://react.dev/)

## Status

| Phase | Description |
|-------|-------------|
| **1** | [Architecture blueprint](docs/architecture/phase-1-architecture-blueprint.md) |
| **2** | Solution scaffold — [details](docs/architecture/phase-2-scaffold.md) |
| **3** | Feature implementation — [details](docs/architecture/phase-3-implementation.md) |
| **4** | Polish & interview-ready (current) — [details](docs/architecture/phase-4-polish.md) |

## Documentation

| Document | Description |
|----------|-------------|
| [Architecture blueprint](docs/architecture/phase-1-architecture-blueprint.md) | Full design reference |
| [Architecture overview](docs/architecture/overview.md) | Short summary + ADRs |
| [Interview talking points](docs/interview/talking-points.md) | Tech demo script |
| [Phase 2 scaffold](docs/architecture/phase-2-scaffold.md) | Solution scaffold |
| [Phase 3 implementation](docs/architecture/phase-3-implementation.md) | Features delivered |
| [Phase 4 polish](docs/architecture/phase-4-polish.md) | UX, validation, tables, tests |

## Tech stack

**Backend:** .NET 8, EF Core, MediatR, FluentValidation, AutoMapper, OpenAPI (Swashbuckle)  
**Frontend:** React 19, Vite, TypeScript, TanStack Query, TanStack Table, Tailwind CSS v4, React Router, React Hook Form, Zod

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- Optional: `dotnet tool install --global dotnet-ef` (for migrations)

> **Note:** Repo includes `nuget.config` using only nuget.org for portable restores (ignores failing corporate feeds).

## Quick start

**Windows — one click:** double-click [`start-oec-ims.bat`](start-oec-ims.bat) (starts API + UI + opens browser).  
**Public UI (GitHub Pages):** push to `main` after [one-time Pages setup](docs/deployment/github-publish.md) — URL: `https://<username>.github.io/<repo-name>/`

```powershell
git clone <your-repo-url>
cd OEC_IMS
.\scripts\dev-setup.ps1
```

**Terminal 1 — API**

```powershell
cd backend\src\OEC.IMS.Api
dotnet run
```

- Swagger: http://localhost:5083/swagger (default `http` profile)
- Status: `GET /api/v1/system/status`
- Health: `GET /health`

**Terminal 2 — SPA**

```powershell
cd frontend
npm run dev
```

- App: http://localhost:5173
- Login: **admin** / `Admin123!` or **clerk** / `Clerk123!`

## Repository layout

```text
OEC_IMS/
├── backend/          # .NET solution (Clean + vertical slices)
├── frontend/         # React 19 SPA
├── docs/             # Architecture, ADRs, interview notes
├── database/         # DB documentation (EF migrations in backend)
├── scripts/          # dev-setup, OpenAPI codegen placeholder
└── .cursor/skills/   # Cursor agent skills for consistent implementation
```

## Cursor skills

Invoke when implementing features:

- `oec-ims-architecture`
- `oec-ims-backend-feature`
- `oec-ims-frontend-feature`
- `oec-ims-api-contract`
- `oec-ims-interview-map`

## License

[MIT](LICENSE)
