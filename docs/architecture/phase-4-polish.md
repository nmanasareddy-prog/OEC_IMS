# Phase 4 — Polish & interview-ready

Phase 4 improves UX, validation parity, and demo narrative without changing core domain rules from Phase 3.

## Frontend

| Area | Delivered |
|------|-----------|
| **Shared UI** | `Modal`, `ConfirmDialog`, `FormField`, `DataTable` (TanStack Table) |
| **Parts** | Zod + React Hook Form (create/edit/adjust); TanStack Table; edit & stock modals; Admin-only delete with confirm |
| **Orders** | Multi-line create form with Zod; TanStack Table; order detail modal; cancel confirm |
| **Auth UX** | JWT role parsing (`jwt.ts`); roles shown in shell; delete gated on `Admin` |

## Backend

| Area | Delivered |
|------|-----------|
| **Unit tests** | FluentValidation tests for Login, CreateOrder, AdjustStock validators |

## Tooling & docs

- OpenAPI codegen placeholder: `scripts/generate-api-client.ps1`
- README updated for Phase 4 and correct local ports (`5083` / `5173`)
- `LICENSE` (MIT)

## Interview talking points

1. **Same validation rules** — Zod on the client mirrors FluentValidation on the server (SKU, lines, stock adjustment).
2. **Role-based UI** — Delete is hidden unless JWT contains `Admin`; API still enforces authorization.
3. **TanStack Table** — Column helpers + shared `DataTable` for consistent list UX across Parts and Orders.
4. **Transactional orders** — Create/cancel flows unchanged from Phase 3; UI now surfaces line-level detail.

## Verify locally

```powershell
cd backend
dotnet test

cd ..\frontend
npm run build

# From repo root
.\start-oec-ims.bat
```

Login: `admin` / `Admin123!` (Admin — can delete parts), `clerk` / `Clerk123!` (no delete).

## Next: Phase 5

See [github-publish.md](../deployment/github-publish.md) for GitHub Pages + hosted API demo.
