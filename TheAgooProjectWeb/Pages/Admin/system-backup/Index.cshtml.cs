using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.Admin.system_backup
{
    [Authorize(Roles = (SD.Role_Admin))]
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public List<SelectListItem> Backups { get; private set; } = new();
        [BindProperty]
        public string? SelectedBackup { get; set; }
        public IndexModel(IWebHostEnvironment env, ILogger<IndexModel> logger, IConfiguration configuration)
        {
            this._env = env;
            this._logger = logger;
            this._config = configuration;
        }
        public void OnGet()
        {
            LoadBackups();
        }
        public void OnPost()
        {
            try
            {
                LoadBackups();
                //var result = RestoreDatabaseFromBackup2("wwwroot/databasebackup/weekly_backup.sql");
                var result = RestoreDatabaseFromBackup(SelectedBackup);
                if (result.Success)
                {
                    TempData["success"] = result.Message;
                }
                else
                {
                    TempData["error"] = result.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("⚠️ DATABASE RESTORE ERROR: {Path}", ex);
                TempData["error"] = "⚠️ DATABASE RESTORE FAILED";
            }
        }


        private void LoadBackups()
        {
            var backupDir = Path.Combine(_env.WebRootPath, "_protected", "mysql-backups");

            if (!Directory.Exists(backupDir))
                return;

            Backups = Directory.GetFiles(backupDir, "*.enc").Select(f => new FileInfo(f)).OrderByDescending(f => f.CreationTimeUtc)
                .Select(f => new SelectListItem
                {
                    Value = f.FullName,   // full path passed to restore
                    Text = $"{f.Name}  ({f.CreationTimeUtc:yyyy-MM-dd HH:mm} UTC)"
                }).ToList();
        }

        public (bool Success, string Message) RestoreDatabaseFromBackup(string backupFilePath)
        {
            string? decryptedFile = null;
            string? sqlFile = null;

            try
            {
                if (string.IsNullOrWhiteSpace(backupFilePath))
                    return (false, "Backup file path is required.");

                var fullPath = Path.GetFullPath(backupFilePath);

                if (!System.IO.File.Exists(fullPath))
                    return (false, $"Backup file not found: {fullPath}");

                if (!fullPath.EndsWith(".enc", StringComparison.OrdinalIgnoreCase))
                    return (false, "Invalid backup format. Expected .enc file.");

                var connStr = _config.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connStr))
                    return (false, "Database connection string is missing.");

                _logger.LogWarning("⚠️ DATABASE RESTORE STARTED from {Path}", fullPath);

                // Temp files
                decryptedFile = Path.Combine(
                    Path.GetTempPath(),
                    $"restore_{Guid.NewGuid():N}.sql.gz"
                );

                sqlFile = decryptedFile.Replace(".gz", string.Empty);

                // 1️⃣ Decrypt
                DecryptFile(fullPath, decryptedFile);

                // 2️⃣ Decompress
                DecompressFile(decryptedFile, sqlFile);

                // 3️⃣ Restore
                using var conn = new MySqlConnection(connStr);
                using var cmd = new MySqlCommand { Connection = conn };
                using var mb = new MySqlBackup(cmd);

                conn.Open();

                // Optional safety
                // mb.ImportInfo.DisableForeignKeys = true;

                mb.ImportFromFile(sqlFile);

                _logger.LogWarning("✅ DATABASE RESTORE COMPLETED SUCCESSFULLY");
                return (true, "Database restore completed successfully.");
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "MySQL error during database restore.");
                return (false, "Database restore failed due to a MySQL error: " + ex.Message);
            }
            catch (CryptographicException)
            {
                return (false, "Failed to decrypt backup. Invalid encryption key or corrupted file.");
            }
            catch (UnauthorizedAccessException)
            {
                return (false, "Access denied while trying to read the backup file.");
            }
            catch (IOException ex)
            {
                return (false, "File I/O error during restore: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during database restore.");
                return (false, "Unexpected error during database restore: " + ex.Message);
            }
            finally
            {
                // Cleanup temp files
                TryDelete(decryptedFile);
                TryDelete(sqlFile);
            }
        }
        private void DecryptFile(string input, string output)
        {
            var key = _config["MySqlBackup:EncryptionKey"];
            if (string.IsNullOrWhiteSpace(key) || key.Length < 32)
                throw new InvalidOperationException("Encryption key is invalid.");

            using var inputFile = System.IO.File.OpenRead(input);

            // Read IV (first 16 bytes)
            byte[] iv = new byte[16];
            inputFile.Read(iv, 0, iv.Length);

            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
            aes.IV = iv;

            using var crypto = new CryptoStream(
                inputFile,
                aes.CreateDecryptor(),
                CryptoStreamMode.Read
            );

            using var outputFile = System.IO.File.Create(output);
            crypto.CopyTo(outputFile);
        }
        private static void DecompressFile(string input, string output)
        {
            using var source = System.IO.File.OpenRead(input);
            using var gzip = new GZipStream(source, CompressionMode.Decompress);
            using var target = System.IO.File.Create(output);

            gzip.CopyTo(target);
        }

        private static void TryDelete(string? path)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(path) && System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }
            catch
            {
                // ignore cleanup failures
            }
        }








        public (bool Success, string Message) RestoreDatabaseFromBackup2(string backupFilePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(backupFilePath))
                {
                    return (false, "Backup file path is required.");
                }

                var fullPath = Path.GetFullPath(backupFilePath);

                if (!System.IO.File.Exists(fullPath))
                {
                    return (false, $"Backup file not found: {fullPath}");
                }

                var connStr = _config.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connStr))
                {
                    return (false, "Database connection string is missing.");
                }

                _logger.LogWarning("⚠️ DATABASE RESTORE STARTED from {Path}", fullPath);

                using var conn = new MySqlConnection(connStr);
                using var cmd = new MySqlCommand { Connection = conn };
                using var mb = new MySqlBackup(cmd);

                conn.Open();

                // Safety: disable foreign keys during restore (optional but recommended)
                // mb.ImportInfo.DisableForeignKeys = true;

                mb.ImportFromFile(fullPath);

                _logger.LogWarning("✅ DATABASE RESTORE COMPLETED SUCCESSFULLY");

                return (true, "Database restore completed successfully.");
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "MySQL error during database restore.");
                return (false, "Database restore failed due to a MySQL error: " + ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return (false, "Access denied while trying to read the backup file.");
            }
            catch (IOException ex)
            {
                return (false, "File I/O error during restore: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during database restore.");
                return (false, "Unexpected error during database restore: " + ex.Message);
            }
        }

    }

}
