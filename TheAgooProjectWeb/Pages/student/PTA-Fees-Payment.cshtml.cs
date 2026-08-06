using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.student
{
    [Authorize(Roles = SD.IsStudent)]
    public class PTA_Fees_PaymentModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
