# MCLauncher Build Scripts

Bu klasörde MCLauncher için build scriptleri bulunmaktadır.

## Scriptler

| Script | Açıklama | Boyut | Gereksinim |
|--------|----------|-------|------------|
| `build-release.bat` | Self-contained tek EXE | ~150-160 MB | Yok |
| `build-small.bat` | Framework-dependent EXE | ~15-20 MB | .NET 10 Runtime |
| `build-trimmed.bat` | Trimmed tek EXE | ~80-100 MB | Yok (riskli) |
| `build-debug.bat` | Debug build | - | - |
| `clean.bat` | Temizlik | - | - |

## Kullanım

1. İstediğiniz `.bat` dosyasına çift tıklayın
2. Build tamamlanınca çıktı konumu gösterilecek

## Çıktı Konumu

```
MCLauncher\bin\Release\net10.0-windows10.0.17763.0\win-x64\publish\MCLauncher.exe
```

## Notlar

- **build-release.bat**: En güvenli seçenek, her yerde çalışır
- **build-small.bat**: Küçük boyut ama hedefte .NET 10 gerekli
- **build-trimmed.bat**: En küçük ama WPF/WinRT sorunları çıkabilir
