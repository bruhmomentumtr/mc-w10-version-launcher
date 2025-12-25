@echo off

echo ========================================
echo   MCLauncher - Clean Build Artifacts
echo ========================================
echo.

:: Get script directory and navigate to project root
cd /d "%~dp0.."
set "CSPROJ=MCLauncher\MCLauncher.csproj"

echo Cleaning via dotnet...
dotnet clean "%CSPROJ%" -v quiet

echo Removing bin and obj folders...
if exist "MCLauncher\bin" rmdir /s /q "MCLauncher\bin"
if exist "MCLauncher\obj" rmdir /s /q "MCLauncher\obj"
if exist "Debug" rmdir /s /q "Debug"
if exist "Release" rmdir /s /q "Release"

echo.
echo Clean complete!
echo.

pause
