using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.Fees_Setup
{
    public class IndexModel : PageModel
    {
        [Authorize(Roles = SD.Role_Admin)]
        public void OnGet()
        {
        }
    }
}
