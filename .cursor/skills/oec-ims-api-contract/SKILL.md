---
name: oec-ims-api-contract
description: >-
  OEC-IMS REST API conventions including versioning, pagination, and RFC 7807
  ProblemDetails. Use when designing endpoints, DTOs, error responses, or
  OpenAPI documentation.
---

# OEC-IMS API Contract

## Base

- Prefix: `/api/v1/`
- JSON request/response
- OpenAPI/Swagger in Development

## Resources (REST)

| Resource | Notes |
|----------|-------|
| `auth` | `POST login` → JWT |
| `parts` | CRUD + `POST {id}/adjust-stock` |
| `vehicles` | Lookups + compatibility link/unlink |
| `orders` | Create, list, detail, cancel |
| `dashboard` | Aggregated read-only metrics |

## Pagination (lists)

Query: `page` (1-based), `pageSize` (max 100), `sort`, `sortDirection`, feature filters.

Response body:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "totalCount": 100,
  "totalPages": 5
}
```

Type: `PagedResult<T>` — not wrapped in extra success envelope.

## Errors (RFC 7807)

- Global middleware maps exceptions to `ProblemDetails`
- Validation failures: `400` with `errors` extension for field keys
- Not found: `404`; conflict (e.g. duplicate SKU): `409`

Do not return generic `{ success: false, message: "..." }` for errors.

## Auth header

`Authorization: Bearer {token}` — mock JWT in V1.

## OpenAPI

- XML comments on controllers and public DTOs
- Document mock JWT security scheme
- Export `openapi.json` for frontend codegen (`scripts/generate-api-client.ps1`)

## Status codes

| Action | Code |
|--------|------|
| Create | 201 + location |
| Update | 200 |
| Delete (soft) | 204 |
| Get | 200 |
| Validation fail | 400 |
