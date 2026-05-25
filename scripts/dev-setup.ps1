# OEC-IMS local development setup
$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $PSScriptRoot

Write-Host "=== OEC-IMS Dev Setup ===" -ForegroundColor Cyan

# Backend
Push-Location "$Root\backend"
dotnet restore
dotnet build
if (-not (Get-Command dotnet-ef -ErrorAction SilentlyContinue)) {
    Write-Host "Installing dotnet-ef tool..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
}
New-Item -ItemType Directory -Force -Path "data" | Out-Null
dotnet ef database update --project src/OEC.IMS.Infrastructure --startup-project src/OEC.IMS.Api
Pop-Location

# Frontend
Push-Location "$Root\frontend"
if (-not (Test-Path "node_modules")) { npm install }
Pop-Location

Write-Host ""
Write-Host "Done. Run in two terminals:" -ForegroundColor Green
Write-Host "  cd backend\src\OEC.IMS.Api && dotnet run"
Write-Host "  cd frontend && npm run dev"
