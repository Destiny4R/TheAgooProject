using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace TheAgooProjectDataAccess
{
    public class MySqlBackupBackgroundService001 : BackgroundService
    {
        private readonly ILogger<MySqlBackupBackgroundService001> _logger;
        private readonly IConfiguration _config;
        private readonly string _backupRoot;

        public MySqlBackupBackgroundService001(
            ILogger<MySqlBackupBackgroundService001> logger,
            IConfiguration config,
            string backupRoot)
        {
            _logger = logger;
            _config = config;
            _backupRoot = backupRoot;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MySQL Backup Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                    await Task.Delay(GetNextDelay(), stoppingToken);
                    await RunBackupWithRetryAsync(stoppingToken);
                    _logger.LogInformation("MySQL backup completed successfully.");
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "MySQL backup service crashed unexpectedly.");
                }
            }
        }

        private TimeSpan GetNextDelay()
        {
            var testMinutes = _config.GetValue<int?>("MySqlBackup:BackupIntervalMinutes");
            if (testMinutes.HasValue && testMinutes.Value > 0)
                return TimeSpan.FromMinutes(testMinutes.Value);

            var now = DateTime.UtcNow;
            int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)now.DayOfWeek + 7) % 7;
            if (daysUntilSunday == 0) daysUntilSunday = 7;

            return now.Date.AddDays(daysUntilSunday) - now;
        }

        private async Task RunBackupWithRetryAsync(CancellationToken ct)
        {
            int maxRetries = _config.GetValue<int>("MySqlBackup:MaxRetries", 3);
            int retryDelay = _config.GetValue<int>("MySqlBackup:RetryDelaySeconds", 15);

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    await Task.Run(DoBackup, ct);

                    return;
                }
                catch (Exception)
                {
                    if (attempt == maxRetries) return;
                    await Task.Delay(TimeSpan.FromSeconds(retryDelay), ct);
                }
            }
        }

        private void DoBackup()
        {
            Directory.CreateDirectory(_backupRoot);

            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
            var rawSql = Path.Combine(_backupRoot, $"mysql_{timestamp}.sql");
            var compressed = rawSql + ".gz";
            var encrypted = compressed + ".enc";

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(
                _config.GetConnectionString("DefaultConnection")))
            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand { Connection = conn })
            using (var mb = new MySqlBackup(cmd))
            {
                conn.Open();
                mb.ExportToFile(rawSql);
            }

            CompressFile(rawSql, compressed);
            EncryptFile(compressed, encrypted);

            File.Delete(rawSql);
            File.Delete(compressed);

            RotateBackups();
        }

        private static void CompressFile(string input, string output)
        {
            using var src = File.OpenRead(input);
            using var dst = File.Create(output);
            using var gzip = new GZipStream(dst, CompressionLevel.Optimal);
            src.CopyTo(gzip);
        }

        private void EncryptFile(string input, string output)
        {
            var key = _config["MySqlBackup:EncryptionKey"];
            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
            aes.GenerateIV();

            using var inFile = File.OpenRead(input);
            using var outFile = File.Create(output);
            outFile.Write(aes.IV);

            using var crypto = new CryptoStream(outFile, aes.CreateEncryptor(), CryptoStreamMode.Write);
            inFile.CopyTo(crypto);
        }

        private void RotateBackups()
        {
            int maxFiles = _config.GetValue<int>("MySqlBackup:MaxBackupFiles", 5);

            var files = Directory.GetFiles(_backupRoot, "*.enc").Select(f => new FileInfo(f)).OrderByDescending(f => f.CreationTimeUtc).Skip(maxFiles);

            foreach (var file in files)
                file.Delete();
        }
    }

}

