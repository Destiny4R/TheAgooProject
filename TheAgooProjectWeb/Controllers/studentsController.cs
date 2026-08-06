using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using System.Security.Claims;

namespace TheAgooProjectWeb.Controllers
{
    [Authorize(Roles = SD.IsStudent)]
    public class studentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<studentsController> logger;

        public studentsController(ApplicationDbContext dbContext, ILogger<studentsController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult TermFeesPayment()
        {
            try
            {
                var claimsId = (ClaimsIdentity)User.Identity;
                var claim = claimsId.FindFirst(ClaimTypes.NameIdentifier);

                var userData = dbContext.Payments
                    .Include(k => k.TermlyFees)
                    .Include(k => k.Termregistrations.StudentsData)
                    .Include(k => k.Termregistrations.SessionYear)
                    .Include(k => k.Termregistrations.Schoolclasses)
                    .Include(k => k.Termregistrations.SubClasses)
                    .Where(k => k.Termregistrations.StudentsData.ApplicationUserId == claim.Value)
                    .ToList();

                userData = userData.OrderByDescending(k => k.CreateDate).ToList();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data = userData.Skip(skip).Take(pageSize).ToList();
                recordsTotal = userData.Count();
                var datax = data.Select(k => new
                {
                    id = k.Id,
                    fullName = $"{k.Termregistrations.StudentsData.SurName} {k.Termregistrations.StudentsData.OtherName} {k.Termregistrations.StudentsData.FirstName}".Replace("  ", " "),
                    studentRegNo = k.Termregistrations.StudentsData.StudentRegNo,
                    fees = SD.ToNaira(k.TermlyFees.Amount),
                    paid = SD.ToNaira(k.Paid),
                    balance = SD.ToNaira(k.Balance),
                    bal = k.Balance,
                    status = k.Status,
                    sessionYear = k.Termregistrations.SessionYear.Name,
                    term = k.Termregistrations.Term,
                    className = $"{k.Termregistrations.Schoolclasses.Name} - {k.Termregistrations.SubClasses.Name}",
                    createDate = k.CreateDate
                }).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = datax };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching term fees payments.");
                throw;
            }
        }

        [HttpPost]
        public IActionResult PTAFeesPaymentTable()
        {
            try
            {
                var claimsId = (ClaimsIdentity)User.Identity;
                var claim = claimsId.FindFirst(ClaimTypes.NameIdentifier);

                var userData = dbContext.PTATables
                    .Include(k => k.Termregistration.SessionYear)
                    .Include(k => k.Termregistration.Schoolclasses)
                    .Include(k => k.Termregistration.SubClasses)
                    .Include(k => k.Termregistration.StudentsData)
                    .Include(k => k.PTAFee)
                    .Where(k => k.Termregistration.StudentsData.ApplicationUserId == claim.Value)
                    .ToList();

                userData = userData.OrderByDescending(k => k.CreatedDate).ToList();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data = userData.Skip(skip).Take(pageSize).ToList();
                recordsTotal = userData.Count();
                var dataz = data.Select(k => new
                {
                    id = k.Id,
                    fullname = $"{k.Termregistration.StudentsData.SurName} {k.Termregistration.StudentsData.OtherName} {k.Termregistration.StudentsData.FirstName}".Replace("  ", " "),
                    regnumber = k.Termregistration.StudentsData.StudentRegNo,
                    amount = SD.ToNaira(k.Amount),
                    balance = SD.ToNaira(k.Balance),
                    bal = k.Balance,
                    status = k.Status,
                    ptafees = SD.ToNaira(k.PTAFee.Fees),
                    session = k.Termregistration.SessionYear.Name,
                    term = k.Termregistration.Term,
                    classname = $"{k.Termregistration.Schoolclasses.Name} - {k.Termregistration.SubClasses.Name}",
                    createdate = k.CreatedDate
                }).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataz };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching PTA fees payments.");
                throw;
            }
        }

        [HttpPost]
        public IActionResult otherPayment()
        {
            try
            {
                var claimsId = (ClaimsIdentity)User.Identity;
                var claim = claimsId.FindFirst(ClaimTypes.NameIdentifier);

                var userData = dbContext.AdditionalPayments
                    .Include(k => k.SpecialPay.OtherPayTables)
                    .Include(i => i.Termregistration.SessionYear)
                    .Include(i => i.Termregistration.Schoolclasses)
                    .Include(i => i.Termregistration.SubClasses)
                    .Include(i => i.Termregistration.StudentsData)
                    .Where(k => k.Termregistration.StudentsData.ApplicationUserId == claim.Value)
                    .ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data = userData.Skip(skip).Take(pageSize).ToList();
                recordsTotal = userData.Count();
                var dataz = data.Select(k => new
                {
                    id = k.Id,
                    fullname = $"{k.Termregistration.StudentsData.SurName} {k.Termregistration.StudentsData.OtherName} {k.Termregistration.StudentsData.FirstName}".Replace("  ", " "),
                    regnumber = k.Termregistration.StudentsData.StudentRegNo,
                    amount = SD.ToNaira(k.SpecialPay.Amount),
                    amount2 = k.SpecialPay.Amount,
                    otherpayitem = k.SpecialPay.OtherPayTables.Name,
                    amountpaid = SD.ToNaira(k.Amount),
                    amountpaid2 = k.Amount,
                    session = k.Termregistration.SessionYear.Name,
                    term = k.Termregistration.Term,
                    classes = $"{k.Termregistration.Schoolclasses.Name} - {k.Termregistration.SubClasses.Name}",
                    createdate = k.CreatedDate
                }).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataz };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching other payments.");
                throw;
            }
        }
    }
}
