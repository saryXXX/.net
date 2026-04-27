#!/bin/bash

# Configuration
BACKEND_DIR="./Backend"
FRONTEND_DIR="./Dashboard/DashboardApp"

echo "🚀 Starting Billing & Stock Management System..."

# 1. Start Backend
echo "📡 Starting Backend (API)..."
cd $BACKEND_DIR
dotnet run --launch-profile http > ../backend.log 2>&1 &
BACKEND_PID=$!
cd ..

# 2. Wait a few seconds for Backend to initialize
sleep 5

# 3. Start Frontend
echo "💻 Starting Frontend (Blazor Dashboard)..."
cd $FRONTEND_DIR
dotnet run --launch-profile http > ../../frontend.log 2>&1 &
FRONTEND_PID=$!
cd ..

echo "---------------------------------------------------"
echo "✅ System is running!"
echo "📡 API: http://localhost:5145"
echo "💻 Dashboard: http://localhost:5146"
echo "---------------------------------------------------"
echo "Press [CTRL+C] to stop both services."

# Trap SIGINT (Ctrl+C) to kill both background processes
trap "echo '🛑 Stopping services...'; kill $BACKEND_PID $FRONTEND_PID; exit" SIGINT

# Keep the script alive
wait
