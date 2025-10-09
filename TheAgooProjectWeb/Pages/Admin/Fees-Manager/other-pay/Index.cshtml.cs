using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.other_pay
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Accountant}")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public IEnumerable<AdditionalPayment> Additionals { get; set; }
        public int IsSearch = 0;
        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            getViewSelect();
        }
        public IActionResult OnPost()
        {
            getViewSelect(); 
            if (ModelState.IsValid)
            {
                Additionals = dbContext.AdditionalPayments.Include(k=>k.SpecialPay).Include(i => i.Termregistration.SessionYear).Include(i => i.Termregistration.Schoolclasses).Include(i => i.Termregistration.SubClasses).Include(i => i.Termregistration.StudentsData).Where(k => k.Termregistration.Term == CTSSClass.Term && k.Termregistration.SessionYearId == CTSSClass.SessionYear && k.Termregistration.ClassesInSchoolId == CTSSClass.Classes && k.Termregistration.SubClassId == CTSSClass.Subclass).ToList();
                if (!Additionals.Any())
                {
                    TempData["error"] = "No result found for the selection made";
                    return Page();
                }
                IsSearch = 3;
                return Page();
            }
            TempData["error"] = "Incomplete selection detected, please all required fields before proceeding";
            return Page();
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
                SessionYearz = dbContext.SessionYears.OrderByDescending(k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                Subjects = dbContext.Subjects.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
