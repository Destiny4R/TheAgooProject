using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer},{SD.Role_Accountant}")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext dbContext;

        public IEnumerable<TermRegistration> Registration { get; set; }
        public IEnumerable<StudentsData> students {  get; set; }
        public IEnumerable<PaymentReport> PaymentReports { get; set; }
        public IEnumerable<PTAFeesReport> pTAFeesReports { get; set; }
        public IEnumerable<ExpenditureTable> Expenditures { get; set; }
        public IEnumerable<PTAExpenditureTable> pTAExpenditures { get; set; }
        public Visit webvisit { get; set; }
        public int feedback { get; set; }
        public string[] _theSession { get; set; }
        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        public void OnGet()
        {
            //string _getvalue = Request?.Cookies[SD.CookiesKey];
            //if (string.IsNullOrEmpty(_getvalue))
            //{
            //    Guid value = Guid.NewGuid();
            //    _getvalue = value.ToString();
            //    Set(SD.CookiesKey, _getvalue.ToString(), TimeSpan.FromMinutes(1440));
            //    var visit = dbContext.Visitors.FirstOrDefault();
            //    if (visit != null)
            //    {
            //        visit.Website+=1;
            //        dbContext.Update(visit);
            //    }
            //    else
            //    {
            //        var vist = new Visit
            //        {
            //            Website = 1,
            //            Portal = 0
            //        };
            //        dbContext.Add(vist);
            //    }
            //    dbContext.SaveChanges();
            //}
            var date2 = DateTime.Now.Year;
            var date1 = DateTime.Now.Year-10;

            Registration = dbContext.TermRegistrations.Include(m=>m.SessionYear).Include(m => m.Schoolclasses).Where(k=>k.CreatedDate.Year > date1 &&  k.CreatedDate.Year >= date2);
            students = dbContext.StudentTable.Where(k => k.CreateDate.Year > date1 && k.CreateDate.Year >= date2);
            PaymentReports = dbContext.PaymentReports.Include(m => m.Termregistration.SessionYear).Include(m => m.Termregistration.Schoolclasses).Where(k => k.CreatedDate.Year > date1 && k.CreatedDate.Year >= date2);
            pTAFeesReports = dbContext.PTAFeesReports.Include(m => m.Termregistration.SessionYear).Include(m => m.Termregistration.Schoolclasses).Where(k => k.CreatedDate.Year > date1 && k.CreatedDate.Year >= date2);
            Expenditures = dbContext.ExpenditureTables.Include(m => m.SessionYear).Where(k => k.CreatedDate.Year > date1 && k.CreatedDate.Year >= date2);
            pTAExpenditures = dbContext.PTAExpenditureTables.Include(j => j.SessionYear).Where(k => k.CreatedDate.Year > date1 && k.CreatedDate.Year >= date2);
            webvisit = dbContext.Visitors.FirstOrDefault();
            feedback = dbContext.FeedBacks.Count();
            var _Session = dbContext.SessionYears.OrderByDescending(k=>k.Name).Take(12).ToList();
            _theSession = new string[_Session.Count()];
            for (int i = 0; i < _Session.Count(); i++)
            {
                _theSession[i] = _Session[i].Name;
            }
        }
        public int _Annual(int year, IEnumerable<StudentsData> students)
        {
            var result =  students.Where(k=>k.CreateDate.Year==year).ToList();
            if (result.Any())
            {
                return result.Count();
            }
            return 0;
        }

        void Set(string key, string value, TimeSpan? expireTime = null)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.Add(expireTime.Value);
            }
            else
            {
                option.Expires = DateTime.Now.AddDays(10);
            }

            option.HttpOnly = true;
            option.Secure = Request.IsHttps;

            Response.Cookies.Append(key, value, option);
        }
        public string GetSession(int minus, int plus)
        {
            var date = DateTime.Now.Year;
            return (date- minus) + "/" + (date - plus);
        }
        public double FeesIncome(string session, IEnumerable<PaymentReport> payments)
        {
            if (!string.IsNullOrEmpty(session) && payments.Any())
            {
                var pay = payments.Where(k=>k.Termregistration.SessionYear.Name==session).ToList();
                double money = 0;
                foreach (var item in pay)
                {
                    money += item.AccummulatedPay;
                }
                return money;
            }
            return 0.0;
        }
        public double FeesExpected(string session, IEnumerable<TermRegistration> termRegistrations)
        {
            if (!string.IsNullOrEmpty(session) && termRegistrations.Any())
            {
                var termreg = termRegistrations.Where(k => k.SessionYear.Name == session).ToList();
                double money = 0;
                foreach (var item in termreg.GroupBy(k=>k.ClassesInSchoolId))
                {
                    foreach (var item1 in item.GroupBy(k=>k.Term))
                    {
                        var register = item1.Count();
                        if(register > 0)
                        {
                            var fees = dbContext.TermLyFeesPays.FirstOrDefault(k=>k.SessionYear.Name == session && k.Term==item1.FirstOrDefault().Term && k.ClassesInSchoolId==item1.FirstOrDefault().ClassesInSchoolId);
                            if(fees != null)
                            {
                                money += (register * fees.Amount);
                            }
                        }
                    }
                }
                return money;
            }
            return 0.0;
        }
        public double Expenses(string session, IEnumerable<ExpenditureTable> expenditures)
        {
            if (!string.IsNullOrEmpty(session) && expenditures.Any())
            {
                var expen = expenditures.Where(k => k.SessionYear.Name == session).ToList();
                double money = 0;
                foreach (var item in expen)
                {
                    money += item.Amount;
                }
                return money;
            }
            return 0.0;
        }
        public double PTAFeesIncome(string session, IEnumerable<PTAFeesReport> payments)
        {
            if (!string.IsNullOrEmpty(session) && payments.Any())
            {
                var pay = payments.Where(k => k.Termregistration.SessionYear.Name == session).ToList();
                double money = 0;
                foreach (var item in pay)
                {
                    money += item.CulmulativePay;
                }
                return money;
            }
            return 0.0;
        }
        public double PTAExpenses(string session, IEnumerable<PTAExpenditureTable> expenditures)
        {
            if (!string.IsNullOrEmpty(session) && expenditures.Any())
            {
                var expen = expenditures.Where(k => k.SessionYear.Name == session).ToList();
                double money = 0;
                foreach (var item in expen)
                {
                    money += item.Amount;
                }
                return money;
            }
            return 0.0;
        }
    }
}
