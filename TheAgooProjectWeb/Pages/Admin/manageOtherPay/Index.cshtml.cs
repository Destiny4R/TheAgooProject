using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.Admin.manageOtherPay
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Accountant}")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
