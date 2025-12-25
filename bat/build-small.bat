@echo off
echo ========================================
echo   MCLauncher - Kucuk EXE Build
echo   (Framework-dependent - .NET gerekli)
echo ========================================
echo.

cd /d "%~dp0.."

echo [1/2] Temizleniyor...
dotnet clean MCLauncher/MCLauncher.csproj -c Release -v quiet

echo [2/2] Framework-dependent build aliniyor...
dotnet publish MCLauncher/MCLauncher.csproj ^
    -c Release ^
    -r win-x64 ^
    --self-contained false ^
    /p:PublishSingleFile=true

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
echo NOT: Bu versiyon icin hedef bilgisayarda .NET 10 Runtime yuklenmis olmali!
echo.

pause
