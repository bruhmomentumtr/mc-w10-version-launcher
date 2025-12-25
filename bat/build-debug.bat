@echo off

echo ========================================
echo   MCLauncher - Debug Build
echo ========================================
echo.

:: Get script directory and navigate to project root
cd /d "%~dp0.."
set "CSPROJ=MCLauncher\MCLauncher.csproj"

echo Building debug version...
dotnet build "%CSPROJ%" -c Debug

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Build failed!
    pause
    exit /b 1
)

echo.
echo ========================================
echo   BUILD COMPLETE!
echo ========================================
echo.

pause
