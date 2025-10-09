using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;
using System.Security.Claims;
using static TheAgooProjectWeb.Pages.Admin.TermRegistration.Batch_RegistrationModel;
using Microsoft.AspNetCore.Authorization;
using TheAgooProjectDataAccess;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.other_pay
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Accountant}")]
    public class add_payModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        
        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public IEnumerable<SelectListItem>? OtherPay { get; set; }
        [BindProperty]
        public ViewModelData modelData { get; set; }
        public int IsSearch = 0;
        public add_payModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            getViewSelect();
            if (id > 0)
            {
                var data = dbContext.AdditionalPayments.Include(k => k.Termregistration.StudentsData).Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.Schoolclasses).Include(k => k.Termregistration.SubClasses).Include(k => k.SpecialPay).FirstOrDefault(k => k.Id==id);
                if (data != null)
                {
                    IsSearch = 3;
                    modelData = new ViewModelData()
                    {
                        FullName = data.Termregistration.StudentsData.SurName + " " + data.Termregistration.StudentsData.OtherName + " " + data.Termregistration.StudentsData.FirstName,
                        SessionYear = data.Termregistration.SessionYear.Name,
                        Term = data.SpecialPay.Term,
                        Classes = data.Termregistration.Schoolclasses.Name,
                        Subclass = data.Termregistration.SubClasses.Name,
                        SpecialpayId = data.SpecialPay.Id,
                        Amount = data.SpecialPay.Amount,
                        StudentReg = data.Termregistration.StudentsData.StudentRegNo,
                        OtherPayType = data.SpecialPay.OtherPayTables.Name,
                        TermRegId = data.Termregistration.Id
                    };
                }
            }
        }
        public IActionResult OnPostSearch(string search, int OtherPayId)
        {
            getViewSelect();
            if (ModelState.IsValid)
            {
                //Check Student Registration
                var TermReg = dbContext.TermRegistrations.Include(k=>k.StudentsData).Include(k => k.Schoolclasses).Include(k => k.SessionYear).Include(k => k.SubClasses).FirstOrDefault(k=>k.ClassesInSchoolId==CTSSClass.Classes && k.SessionYearId==CTSSClass.SessionYear && k.SubClassId==CTSSClass.Subclass && k.StudentsData.StudentRegNo==search && k.Term==CTSSClass.Term);
                if (TermReg == null)
                {
                    TempData["error"] = "The provided student registration number is not registered for the selected information. Please check and try again!";
                    return Page();
                }
                var specialpay = dbContext.SpecialPaySetUps.Include(j=>j.OtherPayTables).FirstOrDefault(k => k.ClassesInSchoolId == CTSSClass.Classes && k.SessionYearId == CTSSClass.SessionYear && k.Term == CTSSClass.Term && k.OtherPayId==OtherPayId);
                if (specialpay == null)
                {
                    TempData["error"] = "No payment set up for the selection made, Please check and try again!";
                    return Page();
                }
                IsSearch = 5;
                modelData = new ViewModelData()
                {
                    FullName = TermReg.StudentsData.SurName + " " + TermReg.StudentsData.OtherName + " " + TermReg.StudentsData.FirstName,
                    SessionYear = TermReg.SessionYear.Name,
                    Term = specialpay.Term,
                    Classes = TermReg.Schoolclasses.Name,
                    Subclass = TermReg.SubClasses.Name,
                    SpecialpayId = specialpay.Id,
                    Amount = specialpay.Amount,
                    StudentReg=TermReg.StudentsData.StudentRegNo,
                    OtherPayType = specialpay.OtherPayTables.Name,
                    TermRegId = TermReg.Id
                };
                return Page();
            }
            return Page();
        }
        public IActionResult OnPostPayment()
        {
            getViewSelect();
            if (modelData.TermRegId>0 && modelData.SpecialpayId>0 && modelData.Amount > 0)
            {
                var UserPay = dbContext.AdditionalPayments.FirstOrDefault(j => j.TermRegId == modelData.TermRegId && j.SpecialpayId==modelData.SpecialpayId);
                if (UserPay != null)
                {
                    TempData["error"] = "Payment already received for this particular student, check and try again.";
                    return Page();
                }


                var ClaimFinder = (ClaimsIdentity)User.Identity;
                var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);

                var pay = new AdditionalPayment()
                {
                    Amount = modelData.Amount,
                    TermRegId = modelData.TermRegId,
                    SpecialpayId = modelData.SpecialpayId,
                    CasheirId = claim.Value
                };
                try
                {
                    dbContext.Add(pay);
                    dbContext.SaveChanges();
                    TempData["success"] = "Payment successfully recorded";
                    return RedirectToPage("Index");
                }catch(Exception ex)
                {
                    TempData["error"] = "Bad error: "+ex.Message;
                }
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

        public class ViewModelData
        {
            public double Amount { get; set; }
            public string? FullName { get; set; }
            public string? StudentReg { get; set; }
            public string? SessionYear { get; set; }
            public string? Term { get; set; }
            public string? Classes { get; set; }
            public string? Subclass { get; set; }
            public int TermRegId { get; set; }
            public string? OtherPayType { get; set; }
            public int SpecialpayId { get; set; }
        }
    }
}
