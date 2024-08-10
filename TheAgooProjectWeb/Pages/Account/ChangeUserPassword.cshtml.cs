using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Account
{
    public class ChangeUserPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Code { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }
        public ChangeUserPasswordModel(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet(string id, string url)
        {
            var getuser = await dbContext.ApplicationUsers.FirstOrDefaultAsync(k => k.Id == id);
            if (getuser != null)
            {
                var code = await userManager.GeneratePasswordResetTokenAsync(getuser);
                Code = code;
                Username = getuser.UserName;
                ReturnUrl = url;
                return Page();
            }
            if (Url.IsLocalUrl(url))
            {
                return RedirectToPage(url);
            }
            return NotFound();
        }

        public async Task<IActionResult> OnPost(string password)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(Username);
                if (user != null && !string.IsNullOrEmpty(password))
                {
                    var result = await userManager.ResetPasswordAsync(user, Code, password);
                    if (result.Succeeded)
                    {
                        TempData["success"] = $"{Username} Password successfully reset";
                        return RedirectToPage(ReturnUrl);
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return Page();
                }
            }
            TempData["error"] = "Missing key information for password update!";
            return Page();
        }
    }
}
