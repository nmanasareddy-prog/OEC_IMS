# Phase 3 — Feature Implementation (Complete)

## Delivered

### Backend (MediatR vertical slices)

| Module | Endpoints |
|--------|-----------|
| **Auth** | `POST /api/v1/auth/login` (mock JWT) |
| **Parts** | CRUD, search, categories, adjust stock |
| **Dashboard** | `GET /api/v1/dashboard/metrics` |
| **Vehicles** | Manufacturers, models, compatible parts, link/unlink |
| **Orders** | Create (transaction + stock deduct), list, detail, cancel pending |

### Infrastructure

- EF migration `InitialCreate`
- Seed data (categories, parts, vehicles, stock, compatibility)
- Auditing on `AuditableEntity`
- Soft delete on parts
- JSON camelCase + string enums

### Frontend

- Real login → JWT in session
- Dashboard KPIs + activity
- Parts table (search, filter, pagination, create, adjust stock)
- Vehicle compatibility search + link
- Orders create / list / cancel

## Demo credentials

| User | Password | Roles |
|------|----------|-------|
| admin | Admin123! | Admin, InventoryClerk |
| clerk | Clerk123! | InventoryClerk |

## Run

```powershell
cd backend\src\OEC.IMS.Api
dotnet run

cd frontend
npm run dev
```

API: http://localhost:5083/swagger · SPA: http://localhost:5173

## Tests

- `dotnet test` — unit + integration (login + parts)
