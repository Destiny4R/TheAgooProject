using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;
using static TheAgooProjectWeb.Pages.Admin.Fees_Manager.Fees_Payment.Add_FeesModel;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.PTA_Fees
{
    [Authorize(Roles =$"{SD.Role_Admin},{SD.Role_Accountant}")]
    public class Make_PaymentModel : PageModel
    {
        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public string namesheet = "";
        private readonly ApplicationDbContext dbContext;

        public ViewSelectModel ViewSelectModel { get; set; }
        public Make_PaymentModel(ApplicationDbContext dbContext)
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
            var getStudent = dbContext.TermRegistrations.Include(j => j.StudentsData).Include(j => j.SessionYear).Include(j => j.Schoolclasses).Include(j => j.SubClasses).FirstOrDefault(k => k.Term == CTSSClass.Term && k.SessionYearId == CTSSClass.SessionYear && k.ClassesInSchoolId == CTSSClass.Classes && k.StudentsData.StudentRegNo == studentReg);
            if (getStudent == null)
            {
                TempData["error"] = "Student with the provided registration nomuber not found or not registered for the selected term";
                return Page();
            }
            var checkifaddedbefore = dbContext.PTAfees.FirstOrDefault(k => k.SessionYearId == CTSSClass.SessionYear && k.Term == CTSSClass.Term);
            if (checkifaddedbefore == null)
            {
                TempData["error"] = "PTA Fees has not been set for the selected term";
                return Page();
            }
            //GET THIS STUDENT PREVIOUS PAYMENT FOR THIS TERM
            var getTotalLastPay = dbContext.PTAFeesReports.Include(m=>m.PTAFees).FirstOrDefault(g => g.TermRegId == getStudent.Id && g.PTAfeesId == checkifaddedbefore.Id);

            viewDataInfo = new ViewDataInfo();
            if (getTotalLastPay != null)
            {
                viewDataInfo.TotalPayment += getTotalLastPay.CulmulativePay;
            }
            viewDataInfo.TotalBalance = checkifaddedbefore.Fees - viewDataInfo.TotalPayment;
            viewDataInfo.Fullname = getStudent.StudentsData.SurName + " " + getStudent.StudentsData.OtherName + " " + getStudent.StudentsData.FirstName;
            viewDataInfo.StudentReg = getStudent.StudentsData.StudentRegNo;
            viewDataInfo.Term = getStudent.Term;
            viewDataInfo.SessionYear = getStudent.SessionYear.Name;
            viewDataInfo.Classes = getStudent.Schoolclasses.Name + " - " + getStudent.SubClasses.Name;
            viewDataInfo.TermRegId = getStudent.Id;
            viewDataInfo.Fees = checkifaddedbefore.Fees;
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
            if (viewDataInfo.Amount > 0 && viewDataInfo.TermRegId > 0)
            {
                var getreport = dbContext.PTAFeesReports.FirstOrDefault(k => k.TermRegId == viewDataInfo.TermRegId);
                if (getreport != null)
                {
                    //EXISTING PAYMENT COMING
                    var getfees = dbContext.PTAfees.AsNoTracking().FirstOrDefault(k => k.Id == viewDataInfo.TermFeesId);
                    if (getreport.CulmulativePay >= getfees.Fees)
                    {
                        TempData["error"] = "PTA fees already completed!";
                        return Page();
                    }
                    getreport.CulmulativePay += viewDataInfo.Amount;
                    if (getreport.CulmulativePay == getfees.Fees || viewDataInfo.TotalBalance < 0)
                    {
                        getreport.Status = SD.Fees_Status_Completed;
                    }
                    else { getreport.Status = SD.Fees_Status_Part_Payment; }
                    dbContext.Update(getreport);
                    var payment = new PTATable
                    {
                        Balance = viewDataInfo.TotalBalance,
                        Amount = viewDataInfo.Amount,
                        PTAFeesId = getfees.Id,
                        TermRegId = viewDataInfo.TermRegId,
                        Description = viewDataInfo.Description,
                        ReceiptNo = SD.GenerateUniqueNumber(),
                        ApplicationUserId = claim.Value
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
                var payreport = new PTAFeesReport
                {
                    CulmulativePay = viewDataInfo.Amount,
                    PTAfeesId = viewDataInfo.TermFeesId,
                    TermRegId = viewDataInfo.TermRegId
                };
                if (viewDataInfo.TotalBalance < 1)
                {
                    payreport.Status = SD.Fees_Status_Completed;
                }
                else { payreport.Status = SD.Fees_Status_Part_Payment; }
                dbContext.Add(payreport);
                var payment2 = new PTATable
                {
                    Balance = viewDataInfo.TotalBalance,
                    Amount = viewDataInfo.Amount,
                    PTAFeesId = viewDataInfo.TermFeesId,
                    TermRegId = viewDataInfo.TermRegId,
                    Description = viewDataInfo.Description,
                    ReceiptNo = SD.GenerateUniqueNumber(),
                    ApplicationUserId = claim.Value
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
    }
}
