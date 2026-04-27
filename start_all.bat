@echo off
title Sary System Startup
echo 🚀 Starting Sary System (Windows Mode)...

:: Check for Backend directory
if not exist "Backend" (
    echo ❌ Error: 'Backend' folder not found! Make sure you are in the project root.
    pause
    exit /b
)

:: 1. Start Backend in a new window
echo 📡 Launching Backend API...
start "Sary API (Backend)" /d "Backend" dotnet run

:: Wait for backend to warm up
echo ⏳ Waiting for API to warm up...
timeout /t 5 /nobreak > NUL

:: 2. Start Frontend in a new window
echo 💻 Launching Dashboard...
if exist "Dashboard\DashboardApp" (
    start "Sary Dashboard" /d "Dashboard\DashboardApp" dotnet run
) else (
    echo ❌ Error: 'Dashboard\DashboardApp' folder not found!
)

echo.
echo ---------------------------------------------------
echo ✅ Services have been launched in separate windows.
echo 🛑 To stop: Close the popup command windows.
echo ---------------------------------------------------
pause
