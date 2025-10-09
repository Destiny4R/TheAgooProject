using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel.ViewModels;
using TheAgooProjectModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.Portal
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer}")]
    public class Print_ResultModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public IEnumerable<TermRegistration> TermReg { get; set; }
        public Print_ResultModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet()
        {
            getViewSelect();
            return Page();
        }

        public IActionResult OnPost()
        {
            getViewSelect();
            if (ModelState.IsValid)
            {
                TermReg = dbContext.TermRegistrations.Include(k => k.StudentsData).Include(k => k.SessionYear).Include(k => k.SubClasses).Include(k => k.Schoolclasses).Where(s => s.SessionYearId == CTSSClass.SessionYear && s.ClassesInSchoolId == CTSSClass.Classes && s.SubClassId == CTSSClass.Subclass && s.Term == CTSSClass.Term).ToList();

                if (TermReg.Count() < 1)
                {
                    TempData["error"] = "Result might not be ready for the select information or wrong combinations, check and try again";
                    return Page();
                }
                return Page();
            }
            TempData["error"] = "Please make a proper select before searching";
            return Page();
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                Class = dbContext.SchoolClasses.OrderByDescending(n => n.Id).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SubClass = dbContext.SubClasses.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SessionYearz = dbContext.SessionYears.OrderBy(n => n.Id).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
