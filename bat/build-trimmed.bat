@echo off
echo ========================================
echo   MCLauncher - Trimmed Build
echo   (En kucuk boyut - Dikkatli kullan!)
echo ========================================
echo.

cd /d "%~dp0.."

echo [1/2] Temizleniyor...
dotnet clean MCLauncher/MCLauncher.csproj -c Release -v quiet

echo [2/2] Trimmed build aliniyor...
dotnet publish MCLauncher/MCLauncher.csproj ^
    -c Release ^
    -r win-x64 ^
    --self-contained true ^
    /p:PublishSingleFile=true ^
    /p:PublishTrimmed=true ^
    /p:TrimMode=partial ^
    /p:IncludeNativeLibrariesForSelfExtract=true ^
    /p:EnableCompressionInSingleFile=true

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [HATA] Build basarisiz!
    echo Trimming bazi WPF/WinRT API'lerini bozabilir.
    echo Hata aliyorsaniz build-release.bat kullanin.
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
echo UYARI: Trimmed build bazi ozellikleri bozabilir!
echo        Test etmeyi unutmayin.
echo.

pause
