# ADR-003: Mock JWT Authentication for V1

## Status

Accepted

## Context

V1 needs protected API routes and role claims for demo realism without integrating Azure AD, Auth0, or Identity Server (scope and setup cost).

## Decision

- `POST /api/v1/auth/login` accepts username/password against **hardcoded demo users**.
- API returns a **signed JWT** (symmetric key from configuration; dev secret in `appsettings.Development.json`, never committed for prod).
- ASP.NET Core JWT bearer authentication validates tokens on protected endpoints.
- **`ICurrentUserService`** abstracts current user id and roles for audit fields and authorization.

## Demo users (V1)

| Username | Password | Roles |
|----------|----------|-------|
| `admin` | `Admin123!` | Admin, InventoryClerk |
| `clerk` | `Clerk123!` | InventoryClerk |

Document credentials in README only (not in source constants committed as production secrets).

## Consequences

### Positive

- Demonstrates auth middleware, policies, and Swagger security scheme.
- Clear upgrade path: replace login handler + register real IdP.

### Negative

- Not suitable for production; must be labeled **mock** in README and interviews.

## Upgrade path (V2)

- Replace mock login with OpenID Connect.
- Keep `ICurrentUserService`; map claims from external token.

## References

- [Phase 1 Architecture Blueprint](../phase-1-architecture-blueprint.md)
