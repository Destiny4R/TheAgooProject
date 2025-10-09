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
    public class analyses_dataModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public SpecialPayTableVM specialPay { get; set; }
        [BindProperty]
        public SpecialDtat special { get; set; }
        public IEnumerable<AdditionalPayment> Additional { get; set; }
        public int hasValue = 0;
        public IEnumerable<SelectListItem>? OtherPay { get; set; }
        public analyses_dataModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public class SpecialDtat
        {
            public int sessionYear { get; set; }
            public int Classes { get; set; }
            public int otherpayId { get; set; }
        }
        public void OnGet()
        {
            getViewSelect();
        }
        
        public IActionResult OnPost(string path)
        {
            getViewSelect();
            if (path == "all")
            {
                Additional = dbContext.AdditionalPayments.Include(m => m.SpecialPay.OtherPayTables).Include(j => j.Termregistration.SessionYear).Include(j => j.Termregistration.Schoolclasses).Where(j => j.Termregistration.SessionYearId == special.sessionYear).ToList();
                if (Additional.Any())
                {
                    hasValue = 3; 
                    return Page();
                }
                TempData["error"] = "Result not found";
                return Page();
            }
            else
            {
                Additional = dbContext.AdditionalPayments.Include(m => m.SpecialPay.OtherPayTables).Include(j => j.Termregistration.SessionYear).Include(j => j.Termregistration.Schoolclasses).Where(j => j.Termregistration.SessionYearId == special.sessionYear && j.Termregistration.ClassesInSchoolId==special.Classes && j.SpecialPay.OtherPayId==special.otherpayId).ToList();
                if (Additional.Any())
                {
                    hasValue = 3;
                    return Page();
                }
                TempData["error"] = "Result not found";
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
                SessionYearz = dbContext.SessionYears.OrderByDescending(k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
            OtherPay = dbContext.OtherPayTables.OrderByDescending(k => k.Name).Select(k => new SelectListItem
            {
                Text = k.Name,
                Value = k.Id.ToString()
            });
        }
    }
}
