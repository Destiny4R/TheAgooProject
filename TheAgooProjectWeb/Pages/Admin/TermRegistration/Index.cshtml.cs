using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Admin.TermRegistration
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer}")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public bool CheckAll2 { get; set; }

        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public CTSSClass CTSSClass { get; set; }

        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            getViewSelect();
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost(List<IncomingData> datas)
        {
            if (datas?.Any(l => l.ischeck) != true)
            {
                TempData["error"] = "No data was sent to the server for processing. Please check and try again.";
                return Page();
            }

            int deleted = 0, skipped = 0;
            var checkedItems = datas.Where(l => l.ischeck).ToList();

            foreach (var data in checkedItems)
            {
                var hasPaymentReport = dbContext.PaymentReports.Any(k => k.TermRegId == data.Id);
                if (hasPaymentReport) { skipped++; continue; }

                var hasPTAFees = dbContext.PTAFeesReports.Any(k => k.TermRegId == data.Id);
                if (hasPTAFees) { skipped++; continue; }

                var hasAdditionalPayment = dbContext.AdditionalPayments.Any(k => k.TermRegId == data.Id);
                if (hasAdditionalPayment) { skipped++; continue; }

                var hasSchoolFees = dbContext.Payments.Any(k => k.TermRegId == data.Id);
                if (hasSchoolFees) { skipped++; continue; }

                var hasComputedResult = dbContext.ResultTable.Any(k => k.TermRegId == data.Id && k.Status);
                if (hasComputedResult) { skipped++; continue; }

                var student = dbContext.TermRegistrations
                    .Include(k => k.ResultTable)
                    .Include(k => k.StudentRatings)
                    .FirstOrDefault(k => k.Id == data.Id);
                if (student == null) { skipped++; continue; }

                if (student.StudentRatings != null)
                    dbContext.Remove(student.StudentRatings);

                if (student.ResultTable?.Any() == true)
                    dbContext.RemoveRange(student.ResultTable);

                dbContext.Remove(student);
                deleted++;
            }

            dbContext.SaveChanges();

            if (deleted > 0)
                TempData["success"] = $"{deleted} student(s) successfully removed from the term.";
            if (skipped > 0)
                TempData["error"] = $"{skipped} student(s) could not be removed due to existing payment or result records.";
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
                })
            };
        }
        public class IncomingData
        {
            public int Id { get; set; }
            public bool ischeck { get; set; }
        }
    }
}
