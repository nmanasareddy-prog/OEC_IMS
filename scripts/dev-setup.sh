#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"

echo "=== OEC-IMS Dev Setup ==="

cd "$ROOT/backend"
dotnet restore
dotnet build
if ! command -v dotnet-ef &>/dev/null; then
  echo "Installing dotnet-ef tool..."
  dotnet tool install --global dotnet-ef
fi
mkdir -p data
dotnet ef database update --project src/OEC.IMS.Infrastructure --startup-project src/OEC.IMS.Api

cd "$ROOT/frontend"
[ -d node_modules ] || npm install

echo ""
echo "Done. Run in two terminals:"
echo "  cd backend/src/OEC.IMS.Api && dotnet run"
echo "  cd frontend && npm run dev"
