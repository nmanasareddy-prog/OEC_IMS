@echo off
setlocal EnableExtensions
title OEC-IMS Launcher

REM Local dev only — double-click to run API + UI. See docs/deployment/github-publish.md for public hosting.
cd /d "%~dp0"
set "ROOT=%~dp0"
set "API_DIR=%ROOT%backend\src\OEC.IMS.Api"
set "UI_DIR=%ROOT%frontend"
set "API_URL=http://localhost:5083"
set "UI_URL=http://localhost:5173"

echo ========================================
echo   OEC Inventory Management System
echo   Starting API + UI for local testing
echo ========================================
echo.

where dotnet >nul 2>&1
if errorlevel 1 goto :ErrDotNet

where npm >nul 2>&1
if errorlevel 1 goto :ErrNode

if not exist "%ROOT%backend\data" mkdir "%ROOT%backend\data"

echo Restoring .NET packages...
pushd "%ROOT%backend"
call dotnet restore
if errorlevel 1 goto :ErrRestore
popd
echo.

if exist "%UI_DIR%\node_modules" goto :SkipNpmInstall
echo Installing frontend packages - first run may take a few minutes...
pushd "%UI_DIR%"
call npm install
if errorlevel 1 goto :ErrNpm
popd
echo.
:SkipNpmInstall

echo [1/2] Starting API at %API_URL%
echo       Swagger: %API_URL%/swagger
start "OEC-IMS API" cmd /k cd /d "%API_DIR%" ^&^& dotnet run --launch-profile http

echo [2/2] Starting UI at %UI_URL%
start "OEC-IMS UI" cmd /k cd /d "%UI_DIR%" ^&^& npm run dev

echo.
echo Waiting for servers...
timeout /t 10 /nobreak >nul

start "" "%UI_URL%"

echo.
echo ========================================
echo   API window title: OEC-IMS API
echo   UI window title:  OEC-IMS UI
echo   Browser:          %UI_URL%
echo.
echo   Login - admin / Admin123!
echo   Login - clerk / Clerk123!
echo.
echo   Stop: close the API and UI command windows.
echo   Public deploy: see docs\deployment\github-publish.md
echo ========================================
echo.
pause
exit /b 0

:ErrDotNet
echo [ERROR] .NET 8 SDK not found. Install from https://dotnet.microsoft.com/download
pause
exit /b 1

:ErrNode
echo [ERROR] Node.js/npm not found. Install Node 20+ from https://nodejs.org/
pause
exit /b 1

:ErrRestore
echo [ERROR] dotnet restore failed.
popd
pause
exit /b 1

:ErrNpm
echo [ERROR] npm install failed.
popd
pause
exit /b 1
