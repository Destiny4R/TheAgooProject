using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.student
{
    [Authorize(Roles = SD.IsStudent)]
    public class Payment_RecordsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
