@echo off
setlocal enabledelayedexpansion

echo ========================================
echo   MCLauncher - Single EXE Build Script
echo ========================================
echo.

:: Get script directory and navigate to project root
cd /d "%~dp0.."
set "PROJECT_ROOT=%cd%"
set "CSPROJ=MCLauncher\MCLauncher.csproj"

:: Detect target framework from csproj
for /f "tokens=2 delims=<>" %%a in ('findstr /i "TargetFramework" "%CSPROJ%"') do set "TFM=%%a"
echo Detected Target Framework: %TFM%
echo.

echo [1/2] Cleaning...
dotnet clean "%CSPROJ%" -c Release -v quiet

echo [2/2] Building Release (self-contained single file)...
dotnet publish "%CSPROJ%" ^
    -c Release ^
    -r win-x64 ^
    --self-contained true ^
    /p:PublishSingleFile=true ^
    /p:IncludeNativeLibrariesForSelfExtract=true ^
    /p:EnableCompressionInSingleFile=true

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

:: Find output path
set "OUTPUT_DIR=MCLauncher\bin\Release\%TFM%\win-x64\publish"
set "OUTPUT_EXE=%OUTPUT_DIR%\MCLauncher.exe"

if exist "%OUTPUT_EXE%" (
    echo Output file:
    echo   %PROJECT_ROOT%\%OUTPUT_EXE%
    echo.
    
    :: Show file size
    for %%A in ("%OUTPUT_EXE%") do (
        set /a "SIZE_MB=%%~zA / 1048576"
        echo Size: !SIZE_MB! MB
    )
) else (
    echo Output directory: %OUTPUT_DIR%
)

echo.
pause
