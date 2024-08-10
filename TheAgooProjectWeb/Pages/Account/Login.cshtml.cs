using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TheAgooProjectDataAccess;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Account
{
    public class LoginModel : PageModel
    {
        public ILogger<LoginModel> Logger { get; }
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public class Inputs
        {
            [Required]
            [StringLength(maximumLength: 20, ErrorMessage = "Username should not exceed 20 characters", MinimumLength = 5)]
            public string Username { get; set; }
            [Required]
            [StringLength(maximumLength: 30)]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            public bool RememberMe { get; set; } = false;
        }
        [BindProperty]
        public Inputs Input { get; set; }
        [BindProperty]
        public string? ReturnUrl { get; set; }

        public LoginModel(ILogger<LoginModel> logger,
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
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(Input.Username);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                        {
                            if (ReturnUrl == "/")
                            {
                                return RedirectToPage("/Index");
                            }
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToPage("/Index");
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Wrong Username or Password");
                    return Page();
                }
                ModelState.AddModelError(string.Empty, "Wrong Username or Password");
                return Page();
            }
            ModelState.AddModelError(string.Empty, "Provide Username and Password");
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
            string password = "Aggo2024";

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
