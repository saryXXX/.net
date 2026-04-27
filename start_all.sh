#!/bin/bash

# Configuration
BACKEND_DIR="./Backend"
FRONTEND_DIR="./Dashboard/DashboardApp"
BACKEND_PORT=5145
FRONTEND_PORT=5232

echo "🚀 Restoring System with Original Ports..."

# Cleanup old processes on these ports to avoid mismanagement
echo "🧹 Cleaning up existing processes on ports $BACKEND_PORT and $FRONTEND_PORT..."
fuser -k $BACKEND_PORT/tcp > /dev/null 2>&1
fuser -k $FRONTEND_PORT/tcp > /dev/null 2>&1
sleep 2

# 1. Start Backend
echo "📡 Starting Backend (API) on http://localhost:$BACKEND_PORT..."
cd $BACKEND_DIR
dotnet run --launch-profile http > ../backend.log 2>&1 &
BACKEND_PID=$!
cd ..

# 2. Wait for Backend
echo "⏳ Waiting for API to warm up..."
sleep 6

# 3. Start Frontend
echo "💻 Starting Frontend (Blazor) on http://localhost:$FRONTEND_PORT..."
cd $FRONTEND_DIR
dotnet run --launch-profile http > ../../frontend.log 2>&1 &
FRONTEND_PID=$!
cd ..

echo "---------------------------------------------------"
echo "✅ System is running with original ports!"
echo "📡 API: http://localhost:$BACKEND_PORT"
echo "💻 Dashboard: http://localhost:$FRONTEND_PORT"
echo "---------------------------------------------------"
echo "Press [CTRL+C] to stop both services."

# Trap SIGINT (Ctrl+C) to kill both background processes
trap "echo '🛑 Stopping services...'; kill $BACKEND_PID $FRONTEND_PID; exit" SIGINT

# Keep the script alive
wait
