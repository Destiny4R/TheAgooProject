using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.PTA_Fees_Setup
{
    [Authorize(Roles = SD.Role_Admin)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
