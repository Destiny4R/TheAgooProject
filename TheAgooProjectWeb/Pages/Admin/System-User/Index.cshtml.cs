using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.Admin.System_User
{
    [Authorize(Roles = (SD.Role_Admin))]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
