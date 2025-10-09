using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.WebsiteManager.WebGallery
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer},{SD.Role_Accountant}")]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public GalleryTable GalleryTable { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            if (id > 0)
            {
                GalleryTable = dbContext.GalleryTables.Include(m => m.Imagestable).FirstOrDefault(k => k.Id == id);
            }
        }
    }
}
