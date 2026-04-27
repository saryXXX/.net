@echo off
title Sary System Startup
echo 🚀 Starting System (Backend + Dashboard)...

:: 1. Start Backend in a new window
echo 📡 Launching Backend API...
start "Backend API" cmd /c "cd Backend && dotnet run"

:: Wait for backend to warm up
echo ⏳ Waiting for API to warm up...
timeout /t 5 /nobreak > NUL

:: 2. Start Frontend in a new window
echo 💻 Launching Dashboard Dashboard...
start "Dashboard App" cmd /c "cd Dashboard\DashboardApp && dotnet run"

echo ---------------------------------------------------
echo ✅ Both services have been launched in separate windows.
echo Close those windows individually to stop the services.
echo ---------------------------------------------------
pause
