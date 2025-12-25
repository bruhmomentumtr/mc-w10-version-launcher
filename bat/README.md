# MCLauncher Build Scripts

Build scripts for the MCLauncher project.

## Scripts

| Script | Description | Output Size | Requirements |
|--------|-------------|-------------|--------------|
| `build-release.bat` | Self-contained single EXE | ~150-160 MB | None |
| `build-small.bat` | Framework-dependent EXE | ~15-20 MB | .NET Runtime |
| `build-trimmed.bat` | Trimmed single EXE | ~80-100 MB | None (risky) |
| `build-debug.bat` | Debug build | - | - |
| `clean.bat` | Clean build artifacts | - | - |

## Usage

1. Double-click any `.bat` file
2. Wait for build to complete
3. Output location will be displayed

## Features

- **Auto-detects target framework** from `.csproj`
- **Shows file size** after build
- **Portable** - works regardless of .NET version

## Notes

- **build-release.bat**: Most reliable, works everywhere
- **build-small.bat**: Smaller but requires .NET Runtime on target
- **build-trimmed.bat**: Smallest but may have WPF/WinRT issues
