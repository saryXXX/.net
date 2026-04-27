#!/bin/bash

echo "🚀 Starting System (Backend + Dashboard)..."

# 1. Start Backend
echo "📡 Launching Backend API..."
cd Backend
dotnet run &
BACKEND_PID=$!
cd ..

# Wait a few seconds for the API to initialize
sleep 5

# 2. Start Frontend
echo "💻 Launching Dashboard Dashboard..."
cd Dashboard/DashboardApp
dotnet run &
FRONTEND_PID=$!
cd ../..

echo "---------------------------------------------------"
echo "✅ Both services are starting in the background."
echo "Press [CTRL+C] to stop everything."
echo "---------------------------------------------------"

# Trap CTRL+C to kill both background processes
trap "echo '🛑 Stopping services...'; kill $BACKEND_PID $FRONTEND_PID; exit" SIGINT

# Wait for both background processes
wait
