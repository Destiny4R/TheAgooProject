using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;

namespace TheAgooProjectWeb.Pages.Admin.Admission
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer},{SD.Role_Accountant}")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<SelectListItem> SessionYearz { get; set; } = Enumerable.Empty<SelectListItem>();

        public void OnGet()
        {
            SessionYearz = dbContext.SessionYears
                .OrderByDescending(k => k.Name)
                .Select(k => new SelectListItem { Text = k.Name, Value = k.Id.ToString() })
                .ToList();
        }
    }
}
