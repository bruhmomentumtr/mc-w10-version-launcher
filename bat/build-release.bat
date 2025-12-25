@echo off
echo ========================================
echo   MCLauncher - Tek EXE Build Script
echo ========================================
echo.

cd /d "%~dp0.."

echo [1/2] Temizleniyor...
dotnet clean MCLauncher/MCLauncher.csproj -c Release -v quiet

echo [2/2] Release build aliniyor...
dotnet publish MCLauncher/MCLauncher.csproj ^
    -c Release ^
    -r win-x64 ^
    --self-contained true ^
    /p:PublishSingleFile=true ^
    /p:IncludeNativeLibrariesForSelfExtract=true ^
    /p:EnableCompressionInSingleFile=true

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
echo Cikti dosyasi:
echo   MCLauncher\bin\Release\net10.0-windows10.0.17763.0\win-x64\publish\MCLauncher.exe
echo.

:: Dosya boyutunu goster
for %%A in ("MCLauncher\bin\Release\net10.0-windows10.0.17763.0\win-x64\publish\MCLauncher.exe") do (
    set size=%%~zA
    set /a sizeMB=%%~zA / 1048576
)
echo Boyut: %sizeMB% MB
echo.

pause
