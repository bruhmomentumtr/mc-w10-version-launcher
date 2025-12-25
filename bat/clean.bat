@echo off
echo ========================================
echo   MCLauncher - Temizlik
echo ========================================
echo.

cd /d "%~dp0.."

echo Temizleniyor...
dotnet clean MCLauncher/MCLauncher.csproj -v quiet

echo bin ve obj klasorleri siliniyor...
rmdir /s /q MCLauncher\bin 2>nul
rmdir /s /q MCLauncher\obj 2>nul
rmdir /s /q Debug 2>nul
rmdir /s /q Release 2>nul

echo.
echo Temizlik tamamlandi!
echo.

pause
