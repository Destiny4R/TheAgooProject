using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TheAgooProjectDataAccess;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Admin.System_User
{
    [Authorize]
    public class UpsertModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        [BindProperty]
        public ApplicationUser user { get; set; }

        public int canEdit = 0;
        public UpsertModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task OnGet(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                user = await userManager.FindByIdAsync(id);
                canEdit = 1;
            }
        }
        public async Task<IActionResult> OnPost(int canEdit)
        {
            if(!string.IsNullOrEmpty(user.Position) && !string.IsNullOrEmpty(user.Fullname))
            {
                if (canEdit > 0)
                {
                    var userx = await userManager.FindByIdAsync(user.Id);
                    if (userx != null)
                    {
                        if (!userx.Position.Equals(user.Position))
                        {
                            var role = await userManager.GetRolesAsync(userx);
                            if (role.Count() > 0)
                            {
                                foreach (var item in role)
                                {
                                    await userManager.RemoveFromRoleAsync(userx, item);
                                }
                            }
                            await userManager.AddToRoleAsync(userx, user.Position);
                        }
                        userx.Fullname = user.Fullname;
                        userx.Position = user.Position;
                        var result = await userManager.UpdateAsync(userx);
                        if (result.Succeeded)
                        {
                            TempData["success"] = "User Account Changes Applied!";
                            return RedirectToPage("Index");
                        }
                    }
                    TempData["success"] = "Unknow user account!";
                    return Page();
                }
                else
                {
                    var employ = await userManager.Users.ToListAsync();
                    var stu_list = employ.Count();
                    string username = "", password = "12345";
                    int sub = DateTime.Now.Year;
                    if (stu_list > 0)
                    {
                        username = $"AGG/STF/" + (10 + stu_list);
                    }
                    var getnewuser = await userManager.FindByNameAsync(username);
                    if (getnewuser != null)
                    {
                        username = $"AGG/STF/" + (10 + stu_list + 1);
                    }
                    var users = new ApplicationUser()
                    {
                        Fullname = user.Fullname,
                        Position = user.Position,
                        UserName = username,
                        Email = username
                    };
                    var result = await userManager.CreateAsync(users, password);
                    if (result.Succeeded)
                    {
                        var result2 = await userManager.AddToRoleAsync(users, user.Position);
                        if (result2.Succeeded)
                        {
                        TempData["success"] = "User Account Successfully Created!";
                        return RedirectToPage("Index");
                        }
                    }
                    else
                    {
                        TempData["success"] = "An error occured while creating user account";
                        return Page();
                    }
                }
            }
            return Page();
        }
    }
}
