# Database

OEC-IMS uses **EF Core code-first migrations** in the backend — not standalone SQL scripts in this folder.

## Location

| Item | Path |
|------|------|
| DbContext | `backend/src/OEC.IMS.Infrastructure/Persistence/ApplicationDbContext.cs` |
| Configurations | `backend/src/OEC.IMS.Infrastructure/Persistence/Configurations/` |
| Migrations | `backend/src/OEC.IMS.Infrastructure/Persistence/Migrations/` |

## Local SQLite file

- Default path: `backend/data/oec-ims.db` (resolved from API project via `../../../data/`; created on first run)
- Gitignored — each developer gets their own file

## Commands

From repository root:

```powershell
cd backend
dotnet ef database update --project src/OEC.IMS.Infrastructure --startup-project src/OEC.IMS.Api
```

Add migration:

```powershell
dotnet ef migrations add <MigrationName> --project src/OEC.IMS.Infrastructure --startup-project src/OEC.IMS.Api
```

## Provider switch

See [ADR-002](../docs/architecture/adr/002-sqlite-with-sql-server-path.md).

Set `Database:Provider` to `SqlServer` and provide `ConnectionStrings:SqlServer` in user secrets or environment.

## Seed data

V1 uses `HasData` in configurations or `IDataSeeder` at startup for:

- Categories
- Manufacturers / models / years
- Demo parts (optional)
