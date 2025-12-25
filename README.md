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
- A Microsoft account connected to Microsoft Store which **owns Minecraft for Windows 10**
- **Administrator permissions** on your user account
- **Developer mode** enabled in Windows Settings
- For beta versions, **Xbox Insider Hub** subscription required

## Setup
1. Download the latest release from the [Releases](https://github.com/bruhmomentumtr/mc-w10-version-launcher/releases) page
2. Run `MCLauncher.exe` to start the launcher

## Building from Source

### Requirements
- .NET 10 SDK

### Build Commands

```powershell
# Debug build
dotnet build MCLauncher/MCLauncher.csproj

# Single EXE (self-contained, ~150MB)
dotnet publish MCLauncher/MCLauncher.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true

# Smaller EXE (requires .NET runtime, ~20MB)
dotnet publish MCLauncher/MCLauncher.csproj -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true
```

Or use the batch scripts in the `bat/` folder:
- `build-release.bat` - Single EXE (~150 MB)
- `build-small.bat` - Small EXE (~20 MB, requires .NET)
- `build-debug.bat` - Debug build

## File Structure

```
MCLauncher.exe
└── DownloadedMCAppx/           ← Downloaded games
    ├── versions_index.json     ← Version index
    ├── Minecraft-1.20.0/       ← Game files
    ├── Minecraft-1.19.0/
    └── ...
```

## FAQ

**Can I run multiple Minecraft instances at the same time?**

No. You can _install_ multiple versions, but only one can run at a time.

## License

Subject to the original project's license.
