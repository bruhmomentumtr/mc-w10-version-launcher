# MCLauncher (Fork)

This tool allows you to install multiple versions of Minecraft: Windows 10 Edition (Bedrock) side-by-side.

> **Note:** This is a fork of [MCMrARM/mc-w10-version-launcher](https://github.com/MCMrARM/mc-w10-version-launcher).

## 🆕 Changes in This Fork

- ✅ **Pure C# Build** - Removed C++ WUTokenHelper dependency
- ✅ **.NET 10** - Modern SDK-style project format
- ✅ **Single EXE** - Single file output with `dotnet publish`
- ✅ **GameStorageManager** - Downloads stored in `DownloadedMCAppx` folder
- ✅ **Improved UX** - Download/removal confirmation dialogs
- ✅ **Version Detection** - Detects existing installations before download

## Disclaimer
This tool will **not** help you to pirate the game; it requires that you have a Microsoft account which can be used to download Minecraft from the Store.

## Prerequisites
- Microsoft account that **owns Minecraft for Windows 10**
- **Administrator permissions**
- **Developer mode** enabled in Windows Settings
- For beta versions: **Xbox Insider Hub** subscription

## Setup
1. Download from [Releases](https://github.com/bruhmomentumtr/mc-w10-version-launcher/releases)
2. Run `MCLauncher.exe`

## Building from Source

### Requirements
- .NET 10 SDK (or version matching `TargetFramework` in `.csproj`)

### Using Batch Scripts

The `bat/` folder contains ready-to-use build scripts:

| Script | Description | Output Size | Requirements |
|--------|-------------|-------------|--------------|
| `build-release.bat` | Self-contained single EXE | ~150 MB | None |
| `build-small.bat` | Framework-dependent EXE | ~20 MB | .NET Runtime |
| `build-trimmed.bat` | Trimmed EXE (experimental) | ~100 MB | None |
| `build-debug.bat` | Debug build | - | - |
| `clean.bat` | Clean artifacts | - | - |

Scripts auto-detect target framework from `.csproj`.

### Manual Build

```powershell
# Debug
dotnet build MCLauncher/MCLauncher.csproj

# Release (self-contained)
dotnet publish MCLauncher/MCLauncher.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true

# Release (small, requires .NET)
dotnet publish MCLauncher/MCLauncher.csproj -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true
```

## File Structure

```
MCLauncher.exe
└── DownloadedMCAppx/           ← Downloaded games
    ├── versions_index.json     ← Version index
    ├── Minecraft-1.20.0/       ← Game files
    └── ...
```

## FAQ

**Can I run multiple instances at the same time?**
No. You can install multiple versions, but only one can run at a time.

## License

Subject to the original project's license.
