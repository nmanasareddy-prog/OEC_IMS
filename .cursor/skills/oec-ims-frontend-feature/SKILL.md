---
name: oec-ims-frontend-feature
description: >-
  Template for OEC-IMS React feature modules using TanStack Query, React Hook
  Form, and Zod. Use when building pages, forms, tables, or API hooks in the
  frontend SPA.
---

# OEC-IMS Frontend Feature Template

Reference: [docs/architecture/phase-1-architecture-blueprint.md](../../docs/architecture/phase-1-architecture-blueprint.md)

## Feature folder

```
src/features/{name}/
  api/           # query keys, useQuery/useMutation hooks
  components/    # feature-specific UI
  pages/         # route entry components
  schemas/       # Zod schemas
  types/         # feature types (if not from OpenAPI codegen)
```

## State rules

| Data | Tool |
|------|------|
| Server | TanStack Query only |
| Auth | `AuthProvider` context |
| Filters | URL search params where shareable |
| Theme | CSS variables |

**Do not** add Zustand/Redux for server data.

## API hooks pattern

```typescript
// api/query-keys.ts
export const partKeys = {
  all: ['parts'] as const,
  list: (filters: PartFilters) => [...partKeys.all, 'list', filters] as const,
  detail: (id: number) => [...partKeys.all, 'detail', id] as const,
};

// api/use-parts.ts — useQuery with typed client from shared/api
```

Invalidate related keys on mutation success (e.g. orders → dashboard + parts).

## Forms

- React Hook Form + `zodResolver(schema)`
- Map 400 ProblemDetails validation errors to `setError`
- Disable submit while `mutation.isPending`

## Tables

- TanStack Table via shared `DataTable`
- Server pagination params match backend: `page`, `pageSize`, `sort`, `sortDirection`

## UI

- Compose from `src/shared/ui/` — no duplicate Button/Modal per feature
- Loading: skeletons; errors: `ErrorState` + retry

## Routes

- Lazy-load feature pages in `app/routes/`
- Protect with auth layout wrapper
