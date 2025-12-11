@echo off
echo ==========================================
echo      FSM Wearable C# Simulator
echo ==========================================

REM Check if local .dotnet exists
if not exist ".dotnet\dotnet.exe" (
    echo [ERROR] Local .NET SDK not found!
    echo Please run the installation step first or ensure .dotnet folder is present.
    pause
    exit /b 1
)

echo.
echo Launching Simulator...
".dotnet\dotnet.exe" run --project FSMSimulator.csproj

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Simulator crashed or failed to start.
    pause
)
