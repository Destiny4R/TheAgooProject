using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using System.Reflection;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Controllers
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer},{SD.Role_Accountant}")]
    public class webapiController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly ISmsSender smsSender;

        public webapiController(ApplicationDbContext dbContext,
            IConfiguration configuration,
            ISmsSender smsSender)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.smsSender = smsSender;
        }
        [AcceptVerbs("POST","GET")]
        public IActionResult Veiwreceipt(int id)
        {
            try 
            {
                var sender = new DetailsPayment();
                string nax;
                if (id > 0)
                {
                    var pay = dbContext.Payments.Include(k=>k.Termregistrations.StudentsData).Include(k => k.Termregistrations.SessionYear).Include(k => k.Termregistrations.Schoolclasses).Include(k => k.Termregistrations.SubClasses).Include(j=>j.TermlyFees).FirstOrDefault(h => h.Id == id);
                    if (pay != null)
                    {
                        var cash = dbContext.ApplicationUsers.FirstOrDefault(i => i.Id == pay.Cashier);
                        sender = new DetailsPayment
                        {
                            Name = pay.Termregistrations.StudentsData.SurName+" "+ pay.Termregistrations.StudentsData.OtherName+" "+ pay.Termregistrations.StudentsData.FirstName,
                            StudentReg = pay.Termregistrations.StudentsData.StudentRegNo,
                            Sessionyear = pay.Termregistrations.SessionYear.Name,
                            Term = pay.Termregistrations.Term,
                            Classes = pay.Termregistrations.Schoolclasses.Name+" - " +  pay.Termregistrations.SubClasses.Name,
                            Fees = SD.ToNaira(pay.TermlyFees.Amount),
                            Amount = SD.ToNaira(pay.Paid),
                            Balance = SD.ToNaira(pay.Balance),
                            Status = pay.Status,
                            Cashier = cash?.Fullname ?? "",
                            Createdate = pay.CreateDate.ToString(),
                            Description = pay.Description,
                            ReceiptNumber = pay.ReceiptNo
                        };
                        return Json(sender);
                    }
                }
                return Json(sender);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Balance()
        {
            string username = "";
            string password = "";
            var intervals = configuration.GetSection("Credentials").Get<List<SmsCredentials>>();
            username = intervals.FirstOrDefault().UserName;
            password = intervals.FirstOrDefault().Password;
            return Json(await smsSender.SendMessage(username, password, null, null, null));
        }
        [AcceptVerbs("Post","Get")]
        public IActionResult SmsDetails(int id)
        {
            if (id > 0)
            {
                var details = dbContext.SmsTable.Include(k => k.ApplicationUser).FirstOrDefault(J => J.Id == id);
                if (details != null)
                {
                    return Json(new { success = true, data=details});
                }
            }
            return Json(new {success = false, message="An error occured while generating this report"});
        }

        //GetPTAFeesPayment
        [AcceptVerbs("Post", "Get")]
        public IActionResult GetPTAFeesPayment(int id)
        {
            if (id > 0)
            {
                var details = dbContext.PTATables.Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.Schoolclasses).Include(k => k.Termregistration.SubClasses).Include(k => k.PTAFee).Include(k => k.Termregistration.StudentsData).FirstOrDefault(J => J.Id == id);
                if (details != null)
                {
                    return Json(new { success = true, data = details });
                }
            }
            return Json(new { success = false, message = "An error occured while generating this report" });
        }
    }

}
