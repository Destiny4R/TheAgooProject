using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TheAgooProjectWeb.Pages.Portal
{
    [Authorize]
    public class Result_SheetModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
