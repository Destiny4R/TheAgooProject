using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.WebsiteManager.WebNews
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Accountant},{SD.Role_ExamOfficer}")]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public NewsTable newsdata { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            if (id > 0)
            {
                newsdata = dbContext.NewsTables.FirstOrDefault(i=>i.Id==id);
            }
        }
    }
}
