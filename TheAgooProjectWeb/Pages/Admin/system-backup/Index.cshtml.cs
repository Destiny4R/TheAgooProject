using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.Admin.system_backup
{
    [Authorize(Roles = (SD.Role_Admin))]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._config = configuration;
        }
        public void OnGet()
        {
        }
        public void OnPost()
        {
            try
            {
                var result = RestoreDatabaseFromBackup("wwwroot/databasebackup/weekly_backup.sql");
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
        public (bool Success, string Message) RestoreDatabaseFromBackup(string backupFilePath)
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
