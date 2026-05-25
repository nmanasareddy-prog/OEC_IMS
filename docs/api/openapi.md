# OpenAPI / API Contract

## Swagger UI

When running the API in Development:

- `https://localhost:<port>/swagger`

## Export OpenAPI document

```powershell
# After API is running, or from build output when configured
./scripts/generate-api-client.ps1
```

Generated client/types target: `frontend/src/shared/api/generated/`

## Contract rules

See [.cursor/skills/oec-ims-api-contract/SKILL.md](../../.cursor/skills/oec-ims-api-contract/SKILL.md) and [phase-1-architecture-blueprint.md](../architecture/phase-1-architecture-blueprint.md).

- Base path: `/api/v1/`
- Pagination: `PagedResult<T>`
- Errors: RFC 7807 ProblemDetails
- Auth: `Authorization: Bearer {jwt}`
