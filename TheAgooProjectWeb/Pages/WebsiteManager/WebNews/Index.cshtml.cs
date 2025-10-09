using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.WebsiteManager.WebNews
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Accountant},{SD.Role_ExamOfficer}")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
