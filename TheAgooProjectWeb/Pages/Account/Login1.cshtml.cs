using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TheAgooProjectDataAccess;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Account
{
    public class Login1Model : PageModel
    {
        public ILogger<Login1Model> Logger { get; }
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        [BindProperty]
        public string? ReturnUrl { get; set; }
        [BindProperty]
        [System.ComponentModel.DataAnnotations.Required]
        public string Username { get; set; }
        public Login1Model(ILogger<Login1Model> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            Logger = logger;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }


        public void OnGet(string? returnUrl)
        {
            var getdata = userManager.Users.Count();
            if (getdata < 1)
            {
                AddAdmin();
            }
            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPost()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                //Get the first character of the username
                var firstChar = Username[0];
                if (firstChar == '#')
                {
                    var newusername = Username.Replace("#", "");
                    var getuser = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == newusername);
                    //Check if the user exists
                    if (getuser != null)
                    {
                        await signInManager.SignInAsync(getuser, true, null);
                        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                        {
                            if (User.IsInRole(SD.IsStudent))
                            {
                                //Redirect to the student dashboard or a specific page for students
                                return RedirectToPage("/student/index");
                            }
                            if (ReturnUrl == "/")
                            {
                                return RedirectToPage("/Index");
                            }
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            if (User.IsInRole(SD.IsStudent))
                            {
                                //Redirect to the student dashboard or a specific page for students
                                return RedirectToPage("/student/index");
                            }
                            return RedirectToPage("/Index");
                        }
                    }
                    else
                    {
                        //Add wrong username message to the model state
                        ModelState.AddModelError("", "Wrong Username or Password");
                        return Page();
                    }
                }

                ModelState.AddModelError("", "Wrong Username or Password");
            }
            return Page();
        }
        private void AddAdmin()
        {
            Addrole();
            string StudentReg = $"AG/ADMIN/01";

            var user = new ApplicationUser();

            user.UserName = StudentReg;
            user.Fullname = "Admin Officer";
            user.Position = SD.Role_Admin;
            string password = "Aggo@2024";

            var result = userManager.CreateAsync(user, password).GetAwaiter().GetResult();
            userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
        }
        private void Addrole()
        {
            if (!roleManager.RoleExistsAsync(SD.Role_ExamOfficer).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Accountant)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_ExamOfficer)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.IsStudent)).GetAwaiter().GetResult();
            }
        }
    }
}
