@echo off
echo ========================================
echo     TIMESHEET TRACKER - STARTUP
echo ========================================
echo.

echo Starting Backend API...
echo.
cd /d "%~dp0src\backend\TimesheetTracker.API"
start "Backend API" cmd /k "dotnet run"

echo Waiting 10 seconds for backend to start...
timeout /t 10 /nobreak >nul

echo.
echo Starting Frontend...
echo.
cd /d "%~dp0src\frontend\TimesheetTracker.Client"
start "Frontend" cmd /k "dotnet run"

echo.
echo ========================================
echo Both applications are starting...
echo.
echo Backend API will be available at:
echo https://localhost:7001 (HTTPS)
echo http://localhost:5000 (HTTP)
echo.
echo Frontend will be available at:
echo https://localhost:7025 (HTTPS)
echo http://localhost:5044 (HTTP)
echo.
echo Wait for both to finish loading, then 
echo open your browser to http://localhost:5044
echo ========================================
pause
