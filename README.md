# MCLauncher (Fork)

Bu araç, Minecraft: Windows 10 Edition (Bedrock) oyununun birden fazla sürümünü yan yana kurmanıza olanak tanır.

> **Not:** Bu, [MCMrARM/mc-w10-version-launcher](https://github.com/MCMrARM/mc-w10-version-launcher) projesinin fork'udur.

## 🆕 Bu Fork'taki Değişiklikler

- ✅ **Pure C# Build** - C++ WUTokenHelper bağımlılığı kaldırıldı
- ✅ **.NET 10** - Modern SDK-style proje formatı
- ✅ **Tek EXE** - `dotnet publish` ile tek dosya çıktısı
- ✅ **GameStorageManager** - İndirmeler `DownloadedMCAppx` klasöründe
- ✅ **Türkçe Arayüz** - İndirme/silme dialogları Türkçe
- ✅ **Gelişmiş Silme** - Tamamen sil veya sadece listeden çıkar seçenekleri

## Uyarı
Bu araç oyunu **korsanlamanıza yardımcı olmaz**; Minecraft'ı Store'dan indirebilecek bir Microsoft hesabınız olması gerekir.

## Gereksinimler
- **Minecraft for Windows 10** sahibi Microsoft hesabı
- **Yönetici izinleri**
- Windows 10 Ayarları'nda **Geliştirici modu** etkin
- Beta sürümler için **Xbox Insider Hub** aboneliği

## Kurulum
1. [Releases](https://github.com/bruhmomentumtr/mc-w10-version-launcher/releases) sayfasından son sürümü indirin
2. `MCLauncher.exe` dosyasını çalıştırın

## Kendiniz Derlemek İçin

### Gereksinimler
- .NET 10 SDK

### Derleme

```powershell
# Debug build
dotnet build MCLauncher/MCLauncher.csproj

# Tek EXE çıktısı (self-contained)
dotnet publish MCLauncher/MCLauncher.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true

# Küçük EXE (.NET runtime gerekli)
dotnet publish MCLauncher/MCLauncher.csproj -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true
```

Veya `bat/` klasöründeki scriptleri kullanabilirsiniz:
- `build-release.bat` - Tek EXE (~150 MB)
- `build-small.bat` - Küçük EXE (~20 MB, .NET gerekli)
- `build-debug.bat` - Debug build

## Dosya Yapısı

```
MCLauncher.exe
└── DownloadedMCAppx/           ← İndirilen oyunlar
    ├── versions_index.json     ← Sürüm index'i
    ├── Minecraft-1.20.0/       ← Oyun dosyaları
    ├── Minecraft-1.19.0/
    └── ...
```

## SSS

**Aynı anda birden fazla Minecraft çalıştırabilir miyim?**

Hayır. Birden fazla sürüm _kurabilirsiniz_, ancak aynı anda yalnızca biri çalışabilir.

## Lisans

Orijinal proje lisansına tabidir.
