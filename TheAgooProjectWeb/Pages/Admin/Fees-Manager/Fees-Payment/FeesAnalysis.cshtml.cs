using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.Fees_Payment
{
    [Authorize(Roles =$"{SD.Role_Admin},{SD.Role_Accountant}")]
    public class FeesAnalysisModel : PageModel
    {
        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public int IsSearch = 0;
        private readonly ApplicationDbContext dbContext;

        public IEnumerable<PaymentReport> PaymentReports { get; set; }
        public IEnumerable<TheAgooProjectModel.TermRegistration> TermReg { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public FeesAnalysisModel(ApplicationDbContext dbContext)
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
            if(CTSSClass.Classes>0 && CTSSClass.SessionYear > 0)
            {
                var report = dbContext.PaymentReports.Include(a => a.TermlyFees).Include(a => a.Termregistration.Schoolclasses).Where(c => c.Termregistration.SessionYearId == CTSSClass.SessionYear && c.Termregistration.ClassesInSchoolId==CTSSClass.Classes).ToList();
                if (report.Any())
                {
                    TermReg = dbContext.TermRegistrations.Include(n => n.Schoolclasses).Include(n => n.SessionYear).Where(d => d.SessionYearId == CTSSClass.SessionYear && d.ClassesInSchoolId==CTSSClass.Classes).ToList();
                    PaymentReports = report;
                    IsSearch = 1;
                }
                return Page();
            }
            else
            {
                var report = dbContext.PaymentReports.Include(a => a.TermlyFees).Include(a => a.Termregistration.Schoolclasses).Where(c => c.Termregistration.SessionYearId == CTSSClass.SessionYear).ToList();
                if (report.Any())
                {
                    TermReg = dbContext.TermRegistrations.Include(n => n.Schoolclasses).Include(n => n.SessionYear).Where(d => d.SessionYearId == CTSSClass.SessionYear).ToList();
                    PaymentReports = report;
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
