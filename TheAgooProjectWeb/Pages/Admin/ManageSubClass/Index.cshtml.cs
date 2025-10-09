using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.Admin.ManageSubClass
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer}")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
