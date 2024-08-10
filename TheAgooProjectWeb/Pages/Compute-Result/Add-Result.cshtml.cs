using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Compute_Result
{
    [Authorize]
    public class Add_ResultModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public SettingNames settingNames;
        public ViewSelectModel ViewSelectModel { get; set; }
        public TermRegistration termreg { get; set; }
        public int hasSomething = 0;
        public Add_ResultModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet()
        {
            var ClaimFinder = (ClaimsIdentity)User.Identity;
            var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);
            getViewSelect();
            termreg = new();
            var settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == claim.Value);
            if (settings != null)
            {
                settingNames = new()
                {
                    Sessionyear = dbContext.SessionYears.Find(settings.sessionYear).Name,
                    Term = settings.Term,
                    Classes = dbContext.SchoolClasses.Find(settings.Classes).Name,
                    Subclass = dbContext.SubClasses.Find(settings.subclass).Name,
                };
                return Page();
            }
            settingNames = new();
            TempData["error"] = "Please go to settings and select the class you will be working on before proceeding!";
            return Page();
        }
        public IActionResult OnPost(string regnumber)
        {
            var ClaimFinder = (ClaimsIdentity)User.Identity;
            var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);
            getViewSelect();
            var settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == claim.Value);
            if (settings != null)
            {
                settingNames = new()
                {
                    Sessionyear = dbContext.SessionYears.Find(settings.sessionYear).Name,
                    Term = settings.Term,
                    Classes = dbContext.SchoolClasses.Find(settings.Classes).Name,
                    Subclass = dbContext.SubClasses.Find(settings.subclass).Name,
                };
            }
            if (!string.IsNullOrEmpty(regnumber))
            {
                var term = dbContext.TermRegistrations.Include(c=>c.Schoolclasses).Include(k=>k.StudentsData).FirstOrDefault(w => w.StudentsData.StudentRegNo == regnumber);
                if (term == null) {
                    TempData["error"] = "Student not found or yet to be register for the term";
                    return Page();
                }
                termreg = term;
                TempData["success"] = "Student found";
                hasSomething = 2;
                return Page();
            }
            TempData["error"] = "Provide student registration number";
            return Page();
        }
        public class SettingNames
        {
            public string Sessionyear { get; set; }
            public string Term { get; set; }
            public string Classes { get; set; }
            public string Subclass { get; set; }
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                Subjects = dbContext.Subjects.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
