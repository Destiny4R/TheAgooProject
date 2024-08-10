using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectDataAccess;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace TheAgooProjectWeb.Pages.Appsettings
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public ViewSelectModel ViewSelectModel { get; set; }
        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public string idz { get; set; }
        public string Message { get; set; }
        public Settings SettingsClass { get; set; }
        public void OnGet()
        {
            getViewSelect();
            var ClaimsId = (ClaimsIdentity)User.Identity;
            var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
            var settingz = dbContext.Settings.Where(m => m.ApplicationUserId == claim.Value).Select(n =>  new CTSSClass
            {
                Classes= (int)n.Classes,
                SessionYear= (int)n.sessionYear,
                Term = n.Term,
                Subclass = (int)n.subclass
            }).FirstOrDefault();
            if (settingz != null)
            {
                CTSSClass = settingz;
            }
        }
        public async Task<IActionResult> OnPost( string? Principal)
        {
            var ClaimsId = (ClaimsIdentity)User.Identity;
            var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(claim.Value))
            {
                var set = dbContext.Settings.FirstOrDefault(o => o.ApplicationUserId == claim.Value);
                if (set == null)
                {
                    if(string.IsNullOrEmpty(CTSSClass.Term) && CTSSClass.SessionYear < 1 && CTSSClass.Classes < 1 && CTSSClass.Subclass < 1)
                    {
                        TempData["error"] = "Make a proper class setting";
                        return RedirectToPage();
                    }
                    Settings settings = new()
                    {
                        Term = CTSSClass.Term,
                        sessionYear = CTSSClass.SessionYear,
                        Classes = CTSSClass.Classes,
                        subclass = CTSSClass.Subclass,
                        ApplicationUserId = claim.Value
                    };
                    if (User.IsInRole(SD.Role_Admin))
                    {
                        if (!string.IsNullOrEmpty(Principal))
                        {
                            settings.PrincipalName = Principal;
                        }
                    }
                    dbContext.Settings.Add(settings);
                }
                else
                {
                    if (!string.IsNullOrEmpty(CTSSClass.Term))
                    {
                        set.Term = CTSSClass.Term;
                    }
                    if (CTSSClass.SessionYear > 0)
                    {
                        set.sessionYear = CTSSClass.SessionYear;
                    }
                    if (CTSSClass.Subclass > 0)
                    {
                        set.subclass = CTSSClass.Subclass;
                    }
                    if (CTSSClass.Classes > 0)
                    {
                        set.Classes = CTSSClass.Classes;
                    }
                    if (!string.IsNullOrEmpty(Principal))
                    {
                        set.PrincipalName = Principal;
                    }
                    dbContext.Update(set);
                }
                int result = dbContext.SaveChanges();
                if (result > 0)
                {
                    TempData["success"] = "Information successfully saved!";
                }
                return RedirectToPage();
            }
            TempData["error"] = "Make a proper class setting";
            return RedirectToPage();
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                Class = dbContext.SchoolClasses.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SubClass = dbContext.SubClasses.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SessionYearz = dbContext.SessionYears.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };

            if (User.IsInRole(SD.Role_Admin))
            {
                var ClaimsId = (ClaimsIdentity)User.Identity;
                var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
                SettingsClass = dbContext.Settings.FirstOrDefault(o => o.ApplicationUserId == claim.Value);
                idz = claim.Value;
                if (SettingsClass != null)
                {
                    if (SettingsClass.CanPrintResult)
                    {
                        Message = "Result available for printing by all student";
                    }
                    else
                    {
                        Message = "Only fees completed students can view and print results";
                    }
                }
            }
        }
    }
}
