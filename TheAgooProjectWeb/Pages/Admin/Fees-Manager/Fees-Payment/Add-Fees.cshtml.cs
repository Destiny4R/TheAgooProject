using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectModel.ViewModels;
using TheAgooProjectModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using TheAgooProjectDataAccess;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.Fees_Payment
{
    [Authorize(Roles =$"{SD.Role_Admin},{SD.Role_Accountant}")]
    public class Add_FeesModel : PageModel
    {
        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public string namesheet = "";
        private readonly ApplicationDbContext dbContext;

        public ViewSelectModel ViewSelectModel { get; set; }
        public Add_FeesModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            getViewSelect();
        }
        public IActionResult OnPostSearch(string studentReg)
        {
            getViewSelect();
            var getStudent = dbContext.TermRegistrations.Include(j=>j.StudentsData).Include(j => j.SessionYear).Include(j => j.Schoolclasses).Include(j => j.SubClasses).FirstOrDefault(k => k.Term == CTSSClass.Term && k.SessionYearId == CTSSClass.SessionYear && k.ClassesInSchoolId == CTSSClass.Classes && k.StudentsData.StudentRegNo == studentReg);
            if (getStudent == null)
            {
                TempData["error"] = "Student with the provided registration nomuber not found or not registered for the selected term";
                return Page();
            }
            var checkifaddedbefore = dbContext.TermLyFeesPays.FirstOrDefault(k => k.SessionYearId == CTSSClass.SessionYear && k.Term == CTSSClass.Term && k.ClassesInSchoolId == CTSSClass.Classes);
            if (checkifaddedbefore == null)
            {
                TempData["error"] = "Fees has not been set for the selected term";
                return Page();
            }
            var getTotalLastPay = dbContext.Payments.Where(g => g.TermRegId == getStudent.Id && g.TermlyFeesId == checkifaddedbefore.Id).ToList();

                viewDataInfo = new ViewDataInfo();
            if (getTotalLastPay.Count() > 0)
            {
                foreach (var item in getTotalLastPay)
                {
                    viewDataInfo.TotalPayment += item.Paid;
                }
            }
            viewDataInfo.TotalBalance = checkifaddedbefore.Amount - viewDataInfo.TotalPayment;
            viewDataInfo.Fullname = getStudent.StudentsData.SurName + " " + getStudent.StudentsData.OtherName + " " + getStudent.StudentsData.FirstName;
            viewDataInfo.StudentReg = getStudent.StudentsData.StudentRegNo;
            viewDataInfo.Term = getStudent.Term;
            viewDataInfo.SessionYear = getStudent.SessionYear.Name;
            viewDataInfo.Classes = getStudent.Schoolclasses.Name + " - " + getStudent.SubClasses.Name;
            viewDataInfo.TermRegId = getStudent.Id;
            viewDataInfo.Fees = checkifaddedbefore.Amount;
            viewDataInfo.TermFeesId = checkifaddedbefore.Id;
        TempData["success"] = "Student found!";
            payx = 3;
            return Page();
        }
        public IActionResult OnPostPayment()
        {
            getViewSelect();
            var ClaimsId = (ClaimsIdentity)User.Identity;
            var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
            if (viewDataInfo.Amount>0 && viewDataInfo.TermRegId > 0)
            {
                var getreport = dbContext.PaymentReports.FirstOrDefault(k => k.TermRegId == viewDataInfo.TermRegId);
                if (getreport != null)
                {
                    //EXISTING PAYMENT COMING
                    var getfees = dbContext.TermLyFeesPays.AsNoTracking().FirstOrDefault(k => k.Id == viewDataInfo.TermFeesId);
                    if(getreport.AccummulatedPay >= getfees.Amount)
                    {
                        TempData["error"] = "Fees already completed!";
                        return Page();
                    }
                    getreport.AccummulatedPay += viewDataInfo.Amount;
                    if (getreport.AccummulatedPay == getfees.Amount || viewDataInfo.TotalBalance < 0)
                    {
                        getreport.Status = SD.Fees_Status_Completed;
                    }
                    else { getreport.Status = SD.Fees_Status_Part_Payment; }
                    dbContext.Update(getreport);
                    var payment = new Payment
                    {
                        Balance = viewDataInfo.TotalBalance,
                        Paid = viewDataInfo.Amount,
                        TermlyFeesId = getfees.Id,
                        TermRegId = viewDataInfo.TermRegId,
                        Description = viewDataInfo.Description,
                        ReceiptNo = SD.GenerateUniqueNumber(),
                        Cashier = claim.Value
                    };
                    if (viewDataInfo.TotalBalance < 1) 
                    {
                        payment.Status = SD.Fees_Status_Completed;
                    }
                    else { payment.Status = SD.Fees_Status_Part_Payment; }
                    dbContext.Add(payment);
                    int result = dbContext.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "Transaction completed successfully!";
                        return RedirectToPage("Index");
                    }
                    TempData["error"] = "An error occured while processing this transaction!";
                    return Page();
                }
                //NEW PAYMENT COMING
                var payreport = new PaymentReport
                {
                    AccummulatedPay= viewDataInfo.Amount,
                    TermlyFeesId= viewDataInfo.TermFeesId,
                    TermRegId = viewDataInfo.TermRegId
                };
                if (viewDataInfo.TotalBalance < 1)
                {
                    payreport.Status = SD.Fees_Status_Completed;
                }
                else { payreport.Status = SD.Fees_Status_Part_Payment; }
                dbContext.Add(payreport);
                var payment2 = new Payment
                {
                    Balance = viewDataInfo.TotalBalance,
                    Paid = viewDataInfo.Amount,
                    TermlyFeesId = viewDataInfo.TermFeesId,
                    TermRegId = viewDataInfo.TermRegId,
                    Description = viewDataInfo.Description,
                    ReceiptNo = SD.GenerateUniqueNumber(),
                    Cashier = claim.Value
                };
                if (viewDataInfo.TotalBalance < 1)
                {
                    payment2.Status = SD.Fees_Status_Completed;
                }
                else { payment2.Status = SD.Fees_Status_Part_Payment; }
                dbContext.Add(payment2);
                int outcome = dbContext.SaveChanges();
                if (outcome > 0)
                {
                    TempData["success"] = "Transaction completed successfully!";
                    return RedirectToPage("Index");
                }
            }
            TempData["error"] = "You have not entered the amount you wish to pay, transaction aborted!";
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
                //SubClass = dbContext.SubClasses.Select(k => new SelectListItem
                //{
                //    Text = k.Name,
                //    Value = k.Id.ToString()
                //}),
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
        [BindProperty]
        public ViewDataInfo viewDataInfo { get; set; }
        public int payx = 0;
        public class ViewDataInfo
        {
            [ValidateNever]
            public string Fullname { get; set; }
            [ValidateNever]
            public string StudentReg { get; set; }
            [ValidateNever]
            public double TotalPayment { get; set; }
            [ValidateNever]
            public double TotalBalance { get; set; }
            [ValidateNever]
            public string SessionYear { get; set; }
            [ValidateNever]
            public string Term { get; set; }
            [ValidateNever]
            public string Classes { get; set; }
            [Required]
            public double Amount { get; set; }
            public int TermRegId { get; set; }
            public int TermFeesId { get; set; }
            public double Fees { get; set; }
            [ValidateNever]
            public string Description { get; set; }
        }
    }
}
