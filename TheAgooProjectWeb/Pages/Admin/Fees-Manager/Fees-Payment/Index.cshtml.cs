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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public int IsSearch = 0;
        public ViewSelectModel ViewSelectModel { get; set; }
        public IEnumerable<PaymentReport> PaymentReports { get; set; }
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
            var reportpay = dbContext.PaymentReports.Include(k=>k.TermlyFees ).Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.Schoolclasses).Include(k => k.Termregistration.SubClasses).Include(k => k.TermlyFees).Include(k => k.Termregistration.StudentsData).Where(h=>h.Termregistration.SessionYearId==CTSSClass.SessionYear && h.Termregistration.Term==CTSSClass.Term && h.Termregistration.ClassesInSchoolId==CTSSClass.Classes && h.Termregistration.SubClassId==CTSSClass.Subclass).ToList();
            if (reportpay.Any())
            {
                PaymentReports = reportpay;
                IsSearch = 2;
            }
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
                SessionYearz = dbContext.SessionYears.OrderByDescending (k => k.Name).Select(k => new SelectListItem
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
