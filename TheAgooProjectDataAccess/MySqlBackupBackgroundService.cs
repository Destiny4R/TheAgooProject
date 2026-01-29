using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Diagnostics;

namespace TheAgooProjectDataAccess
{
    public class MySqlBackupBackgroundService : BackgroundService
    {
        private readonly ILogger<MySqlBackupBackgroundService> _logger;
        private readonly IConfiguration _config;

        public MySqlBackupBackgroundService(ILogger<MySqlBackupBackgroundService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MySQL Backup Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var delay = GetNextDelay();
                    _logger.LogInformation("Next MySQL backup in {Minutes} minutes", delay.TotalMinutes);

                    await Task.Delay(delay, stoppingToken);

                    await RunBackupWithRetryAsync(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // App shutting down
                }
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

            // Production: Weekly Sunday 12:00 AM UTC
            var now = DateTime.UtcNow;

            int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)now.DayOfWeek + 7) % 7;
            if (daysUntilSunday == 0)
                daysUntilSunday = 7;

            var nextRun = now.Date.AddDays(daysUntilSunday);
            return nextRun - now;
        }

        private async Task RunBackupWithRetryAsync(CancellationToken ct)
        {
            int maxRetries = _config.GetValue<int>("MySqlBackup:MaxRetries", 3);
            int retryDelay = _config.GetValue<int>("MySqlBackup:RetryDelaySeconds", 15);

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    _logger.LogInformation("Starting MySQL backup attempt {Attempt}/{Max}", attempt, maxRetries);

                    await Task.Run(() => DoBackup(), ct);

                    _logger.LogInformation("MySQL backup completed successfully.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "MySQL backup attempt {Attempt} failed.", attempt);

                    if (attempt == maxRetries)
                    {
                        _logger.LogCritical("All MySQL backup attempts failed.");
                        return; // do NOT crash app
                    }

                    await Task.Delay(TimeSpan.FromSeconds(retryDelay), ct);
                }
            }
        }

        private void DoBackup()
        {
            var backupPath = _config["MySqlBackup:BackupPath"];
            if (string.IsNullOrWhiteSpace(backupPath))
                throw new InvalidOperationException("MySqlBackup:BackupPath is not configured.");

            var fullPath = Path.GetFullPath(backupPath);

            var dir = Path.GetDirectoryName(fullPath)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var connStr = _config.GetConnectionString("DefaultConnection");

            using var conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            using var cmd = new MySql.Data.MySqlClient.MySqlCommand { Connection = conn };
            using var mb = new MySql.Data.MySqlClient.MySqlBackup(cmd);

            conn.Open();                 // can fail
            mb.ExportToFile(fullPath);   // can fail
        }
    }


}
