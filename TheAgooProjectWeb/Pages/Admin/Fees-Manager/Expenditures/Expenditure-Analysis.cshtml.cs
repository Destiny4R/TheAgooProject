using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;
using static TheAgooProjectWeb.Pages.Admin.TermRegistration.Batch_RegistrationModel;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.Expenditures
{
    [Authorize(Roles =$"{SD.Role_Admin},{SD.Role_Accountant}")]
    public class Expenditure_AnalysisModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public IEnumerable<ExpenditureTable> expenditures { get; set; }
        public int IsSearch = 0;
        public Expenditure_AnalysisModel(ApplicationDbContext dbContext)
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
            if (!string.IsNullOrEmpty(CTSSClass.Term) && CTSSClass.SessionYear > 0)
            {
                var report = dbContext.ExpenditureTables.Include(a => a.expenditureTypex).Where(c => c.SessionYearId == CTSSClass.SessionYear && c.Term == CTSSClass.Term).ToList();
                if (report.Any())
                {
                    expenditures = report;
                    IsSearch = 1;
                }
                return Page();
            }
            else
            {
                var report = dbContext.ExpenditureTables.Include(a => a.expenditureTypex).Where(c => c.SessionYearId == CTSSClass.SessionYear).ToList();
                if (report.Any())
                {
                    expenditures = report;
                    IsSearch = 1;
                }
                return Page();
            }
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
                SessionYearz = dbContext.SessionYears.OrderByDescending (k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
