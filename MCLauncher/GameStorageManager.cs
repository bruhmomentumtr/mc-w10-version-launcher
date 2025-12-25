using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MCLauncher
{
    /// <summary>
    /// İndirilen Minecraft sürümlerini yöneten sınıf.
    /// Tüm indirmeler EXE'nin yanındaki "DownloadedMCAppx" klasöründe saklanır.
    /// </summary>
    public class GameStorageManager
    {
        private static readonly string STORAGE_FOLDER_NAME = "DownloadedMCAppx";
        private static readonly string INDEX_FILE_NAME = "versions_index.json";

        private static GameStorageManager _instance;
        public static GameStorageManager Instance => _instance ??= new GameStorageManager();

        /// <summary>
        /// İndirilen sürümlerin saklandığı ana klasör
        /// </summary>
        public string StoragePath { get; private set; }

        /// <summary>
        /// Sürüm indeksinin saklandığı dosya
        /// </summary>
        public string IndexFilePath { get; private set; }

        /// <summary>
        /// İndirilen sürümlerin listesi
        /// </summary>
        public VersionIndex Index { get; private set; }

        private GameStorageManager()
        {
            Initialize();
        }

        /// <summary>
        /// Başlatma - klasör ve index dosyasını oluşturur/yükler
        /// </summary>
        private void Initialize()
        {
            // EXE'nin bulunduğu klasörü al
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            StoragePath = Path.Combine(exePath, STORAGE_FOLDER_NAME);
            IndexFilePath = Path.Combine(StoragePath, INDEX_FILE_NAME);

            // Klasör yoksa oluştur
            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
                Debug.WriteLine($"GameStorageManager: Klasör oluşturuldu: {StoragePath}");
            }

            // Index dosyasını yükle veya oluştur
            LoadIndex();

            // Bütünlük kontrolü yap
            VerifyIntegrity();
        }

        /// <summary>
        /// Index dosyasını yükler
        /// </summary>
        private void LoadIndex()
        {
            if (File.Exists(IndexFilePath))
            {
                try
                {
                    string json = File.ReadAllText(IndexFilePath);
                    Index = JsonConvert.DeserializeObject<VersionIndex>(json) ?? new VersionIndex();
                    Debug.WriteLine($"GameStorageManager: Index yüklendi, {Index.Versions.Count} sürüm bulundu");
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"GameStorageManager: Index yüklenemedi: {e.Message}");
                    Index = new VersionIndex();
                }
            }
            else
            {
                Index = new VersionIndex();
                SaveIndex();
            }
        }

        /// <summary>
        /// Index dosyasını kaydeder
        /// </summary>
        public void SaveIndex()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Index, Formatting.Indented);
                File.WriteAllText(IndexFilePath, json);
                Debug.WriteLine($"GameStorageManager: Index kaydedildi");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"GameStorageManager: Index kaydedilemedi: {e.Message}");
            }
        }

        /// <summary>
        /// Klasördeki dosyaları index ile karşılaştırır ve tutarsızlıkları düzeltir
        /// </summary>
        public void VerifyIntegrity()
        {
            Debug.WriteLine("GameStorageManager: Bütünlük kontrolü başlatılıyor...");

            // Index'teki ama klasörde olmayan sürümleri bul
            var missingVersions = Index.Versions
                .Where(v => !Directory.Exists(Path.Combine(StoragePath, v.FolderName)))
                .ToList();

            foreach (var missing in missingVersions)
            {
                Debug.WriteLine($"GameStorageManager: Eksik sürüm kaldırılıyor: {missing.Name}");
                Index.Versions.Remove(missing);
            }

            // Klasörde olup index'te olmayan klasörleri bul
            if (Directory.Exists(StoragePath))
            {
                var directories = Directory.GetDirectories(StoragePath);
                foreach (var dir in directories)
                {
                    string folderName = Path.GetFileName(dir);
                    if (!Index.Versions.Any(v => v.FolderName == folderName))
                    {
                        // AppxManifest.xml varsa geçerli bir Minecraft kurulumu
                        if (File.Exists(Path.Combine(dir, "AppxManifest.xml")))
                        {
                            Debug.WriteLine($"GameStorageManager: Yetim klasör bulundu, index'e ekleniyor: {folderName}");
                            Index.Versions.Add(new VersionEntry
                            {
                                Name = folderName,
                                FolderName = folderName,
                                DownloadDate = Directory.GetCreationTime(dir),
                                IsRegistered = false
                            });
                        }
                    }
                }
            }

            if (missingVersions.Count > 0)
            {
                SaveIndex();
            }

            Debug.WriteLine($"GameStorageManager: Bütünlük kontrolü tamamlandı. Toplam {Index.Versions.Count} sürüm.");
        }

        /// <summary>
        /// Yeni bir sürüm için klasör yolu döndürür
        /// </summary>
        public string GetVersionPath(string versionName, bool isPreview)
        {
            string folderName = (isPreview ? "Minecraft-Preview-" : "Minecraft-") + versionName;
            return Path.Combine(StoragePath, folderName);
        }

        /// <summary>
        /// İndirilen paket dosyası için yol döndürür
        /// </summary>
        public string GetPackagePath(string versionName, bool isPreview, bool isGDK)
        {
            string fileName = (isPreview ? "Minecraft-Preview-" : "Minecraft-") + versionName + (isGDK ? ".msixvc" : ".Appx");
            return Path.Combine(StoragePath, fileName);
        }

        /// <summary>
        /// Sürümü index'e ekler
        /// </summary>
        public void AddVersion(string name, string folderName, bool isPreview, bool isGDK)
        {
            // Zaten varsa güncelle
            var existing = Index.Versions.FirstOrDefault(v => v.FolderName == folderName);
            if (existing != null)
            {
                existing.DownloadDate = DateTime.Now;
                existing.IsPreview = isPreview;
                existing.IsGDK = isGDK;
            }
            else
            {
                Index.Versions.Add(new VersionEntry
                {
                    Name = name,
                    FolderName = folderName,
                    DownloadDate = DateTime.Now,
                    IsPreview = isPreview,
                    IsGDK = isGDK,
                    IsRegistered = false
                });
            }

            SaveIndex();
            Debug.WriteLine($"GameStorageManager: Sürüm eklendi: {name}");
        }

        /// <summary>
        /// Sürümü index'ten kaldırır
        /// </summary>
        public void RemoveVersion(string folderName, bool deleteFiles)
        {
            var version = Index.Versions.FirstOrDefault(v => v.FolderName == folderName);
            if (version != null)
            {
                Index.Versions.Remove(version);
                SaveIndex();
                Debug.WriteLine($"GameStorageManager: Sürüm index'ten kaldırıldı: {version.Name}");
            }

            if (deleteFiles)
            {
                string fullPath = Path.Combine(StoragePath, folderName);
                if (Directory.Exists(fullPath))
                {
                    try
                    {
                        Directory.Delete(@"\\?\" + Path.GetFullPath(fullPath), true);
                        Debug.WriteLine($"GameStorageManager: Klasör silindi: {fullPath}");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"GameStorageManager: Klasör silinemedi: {e.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Sürümün kayıtlı olup olmadığını günceller
        /// </summary>
        public void SetRegistered(string folderName, bool isRegistered)
        {
            var version = Index.Versions.FirstOrDefault(v => v.FolderName == folderName);
            if (version != null)
            {
                version.IsRegistered = isRegistered;
                SaveIndex();
            }
        }

        /// <summary>
        /// İndirilen tüm sürümlerin listesi
        /// </summary>
        public List<VersionEntry> GetAllVersions()
        {
            VerifyIntegrity();
            return Index.Versions.ToList();
        }

        /// <summary>
        /// Sürümün mevcut olup olmadığını kontrol eder
        /// </summary>
        public bool VersionExists(string folderName)
        {
            string fullPath = Path.Combine(StoragePath, folderName);
            return Directory.Exists(fullPath) && File.Exists(Path.Combine(fullPath, "AppxManifest.xml"));
        }
    }

    /// <summary>
    /// Sürüm index dosyası yapısı
    /// </summary>
    public class VersionIndex
    {
        public List<VersionEntry> Versions { get; set; } = new List<VersionEntry>();
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Tek bir sürüm kaydı
    /// </summary>
    public class VersionEntry
    {
        public string Name { get; set; }
        public string FolderName { get; set; }
        public DateTime DownloadDate { get; set; }
        public bool IsPreview { get; set; }
        public bool IsGDK { get; set; }
        public bool IsRegistered { get; set; }
    }
}
