# Placeholder: generate TypeScript client from OpenAPI when API is running.
# Example: npx openapi-typescript https://localhost:7001/swagger/v1/swagger.json -o frontend/src/shared/api/generated/schema.d.ts
$ErrorActionPreference = "Stop"
Write-Host "Start the API, then run openapi-typescript against /swagger/v1/swagger.json" -ForegroundColor Yellow
Write-Host "See docs/api/openapi.md"
