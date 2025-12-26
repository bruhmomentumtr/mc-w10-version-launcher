# MCLauncher Build Scripts

Build scripts for the MCLauncher project.

## Scripts

| Script | Description | Size | Startup |
|--------|-------------|------|---------|
| `build-release.bat` | Compressed single EXE | ~67 MB | Slower |
| `build-release-uncompressed.bat` | Uncompressed single EXE | ~150 MB | Faster |
| `clean.bat` | Clean build artifacts | - | - |

## Usage

1. Double-click any `.bat` file
2. Wait for build to complete
3. Output: `MCLauncher\bin\Release\...\publish\MCLauncher.exe`

## Notes

- **Compressed**: Smaller file, but slightly slower first startup
- **Uncompressed**: Larger file, but faster startup
