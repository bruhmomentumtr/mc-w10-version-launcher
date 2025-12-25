@echo off
echo ========================================
echo   MCLauncher - Debug Build
echo ========================================
echo.

cd /d "%~dp0.."

echo Building debug version...
dotnet build MCLauncher/MCLauncher.csproj -c Debug

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [HATA] Build basarisiz!
    pause
    exit /b 1
)

echo.
echo ========================================
echo   BUILD TAMAMLANDI!
echo ========================================
echo.

pause
