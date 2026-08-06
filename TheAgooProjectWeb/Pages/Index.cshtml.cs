using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer},{SD.Role_Accountant},{SD.IsStudent}")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public IEnumerable<TermRegistration> Registration { get; set; } = new List<TermRegistration>();
        public IEnumerable<StudentsData> students { get; set; } = new List<StudentsData>();
        public IEnumerable<PaymentReport> PaymentReports { get; set; } = new List<PaymentReport>();
        public IEnumerable<PTAFeesReport> pTAFeesReports { get; set; } = new List<PTAFeesReport>();
        public IEnumerable<ExpenditureTable> Expenditures { get; set; } = new List<ExpenditureTable>();
        public IEnumerable<PTAExpenditureTable> pTAExpenditures { get; set; } = new List<PTAExpenditureTable>();

        public Visit webvisit { get; set; } = null!;
        public int feedback { get; set; } = 0;
        public string[] _theSession { get; set; } = Array.Empty<string>();

        // Dynamic Dashboard Metrics & User Information
        public string LoggedInUserFullName { get; set; } = "Administrator";
        public string CurrentSessionName { get; set; } = string.Empty;
        public string CurrentTermName { get; set; } = string.Empty;
        public int TotalActiveStudents { get; set; } = 0;
        public int TotalTermRegistrations { get; set; } = 0;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole(SD.IsStudent))
            {
                return RedirectToPage("/student/index");
            }

            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser != null && !string.IsNullOrWhiteSpace(currentUser.Fullname))
            {
                LoggedInUserFullName = currentUser.Fullname;
            }
            else
            {
                LoggedInUserFullName = User.Identity?.Name ?? "Administrator";
            }

            int date1 = DateTime.Now.Year - 10;

            // Fetch records for the past 10 years without restricting to current year only
            Registration = await dbContext.TermRegistrations.AsNoTracking()
                .Include(m => m.SessionYear)
                .Include(m => m.Schoolclasses)
                .Where(k => k.CreatedDate.Year >= date1)
                .ToListAsync();

            students = await dbContext.StudentTable.AsNoTracking()
                .Where(k => k.CreateDate.Year >= date1)
                .ToListAsync();

            PaymentReports = await dbContext.PaymentReports.AsNoTracking()
                .Include(m => m.Termregistration.SessionYear)
                .Include(m => m.Termregistration.Schoolclasses)
                .Where(k => k.CreatedDate.Year >= date1)
                .ToListAsync();

            pTAFeesReports = await dbContext.PTAFeesReports.AsNoTracking()
                .Include(m => m.Termregistration.SessionYear)
                .Include(m => m.Termregistration.Schoolclasses)
                .Where(k => k.CreatedDate.Year >= date1)
                .ToListAsync();

            Expenditures = await dbContext.ExpenditureTables.AsNoTracking()
                .Include(m => m.SessionYear)
                .Where(k => k.CreatedDate.Year >= date1)
                .ToListAsync();

            pTAExpenditures = await dbContext.PTAExpenditureTables.AsNoTracking()
                .Include(j => j.SessionYear)
                .Where(k => k.CreatedDate.Year >= date1)
                .ToListAsync();

            webvisit = await dbContext.Visitors.AsNoTracking().FirstOrDefaultAsync() ?? new Visit { Website = 0, Portal = 0 };
            feedback = await dbContext.FeedBacks.AsNoTracking().CountAsync();

            TotalActiveStudents = await dbContext.StudentTable.AsNoTracking().CountAsync(s => s.IsActive);

            // Dynamically determine current active session and term
            var latestGeneralClass = await dbContext.GeneralClassTables.AsNoTracking()
                .Include(g => g.SessionYear)
                .OrderByDescending(g => g.Id)
                .FirstOrDefaultAsync();

            if (latestGeneralClass != null)
            {
                CurrentSessionName = latestGeneralClass.SessionYear?.Name ?? "2025/2026";
                CurrentTermName = latestGeneralClass.Term ?? "Third";
            }
            else
            {
                var latestSession = await dbContext.SessionYears.AsNoTracking()
                    .OrderByDescending(s => s.Id)
                    .FirstOrDefaultAsync();
                CurrentSessionName = latestSession?.Name ?? $"{DateTime.Now.Year - 1}/{DateTime.Now.Year}";
                CurrentTermName = "Third";
            }

            var currentSessionYearObj = await dbContext.SessionYears.AsNoTracking()
                .FirstOrDefaultAsync(s => s.Name == CurrentSessionName);

            if (currentSessionYearObj != null)
            {
                TotalTermRegistrations = await dbContext.TermRegistrations.AsNoTracking()
                    .CountAsync(r => r.SessionYearId == currentSessionYearObj.Id);
            }
            else
            {
                TotalTermRegistrations = Registration.Count();
            }

            var sessionList = await dbContext.SessionYears.AsNoTracking()
                .OrderByDescending(k => k.Name)
                .Take(12)
                .ToListAsync();

            _theSession = sessionList.Select(s => s.Name).ToArray();

            return Page();
        }

        public int _Annual(int year, IEnumerable<StudentsData> students)
        {
            if (students == null) return 0;
            return students.Count(k => k.CreateDate.Year == year);
        }

        public string GetSession(int minus, int plus)
        {
            int date = DateTime.Now.Year;
            return $"{date - minus}/{date - plus}";
        }

        public double FeesIncome(string session, IEnumerable<PaymentReport> payments)
        {
            if (string.IsNullOrEmpty(session) || payments == null || !payments.Any())
                return 0.0;

            return payments
                .Where(k => k.Termregistration != null && k.Termregistration.SessionYear != null && k.Termregistration.SessionYear.Name == session)
                .Sum(item => item.AccummulatedPay);
        }

        public double FeesExpected(string session, IEnumerable<TermRegistration> termRegistrations)
        {
            if (string.IsNullOrEmpty(session) || termRegistrations == null || !termRegistrations.Any())
                return 0.0;

            var termreg = termRegistrations.Where(k => k.SessionYear != null && k.SessionYear.Name == session).ToList();
            if (!termreg.Any()) return 0.0;

            double money = 0;
            var feeRates = dbContext.TermLyFeesPays.AsNoTracking()
                .Include(f => f.SessionYear)
                .Where(f => f.SessionYear.Name == session)
                .ToList();

            foreach (var classGroup in termreg.GroupBy(k => k.ClassesInSchoolId))
            {
                foreach (var termGroup in classGroup.GroupBy(k => k.Term))
                {
                    int count = termGroup.Count();
                    if (count > 0)
                    {
                        string currentTerm = termGroup.Key;
                        int classId = classGroup.Key;

                        var feeSetting = feeRates.FirstOrDefault(k => k.Term == currentTerm && k.ClassesInSchoolId == classId);
                        if (feeSetting != null)
                        {
                            money += count * feeSetting.Amount;
                        }
                    }
                }
            }
            return money;
        }

        public double Expenses(string session, IEnumerable<ExpenditureTable> expenditures)
        {
            if (string.IsNullOrEmpty(session) || expenditures == null || !expenditures.Any())
                return 0.0;

            return expenditures
                .Where(k => k.SessionYear != null && k.SessionYear.Name == session)
                .Sum(item => item.Amount);
        }

        public double PTAFeesIncome(string session, IEnumerable<PTAFeesReport> payments)
        {
            if (string.IsNullOrEmpty(session) || payments == null || !payments.Any())
                return 0.0;

            return payments
                .Where(k => k.Termregistration != null && k.Termregistration.SessionYear != null && k.Termregistration.SessionYear.Name == session)
                .Sum(item => item.CulmulativePay);
        }

        public double PTAExpenses(string session, IEnumerable<PTAExpenditureTable> expenditures)
        {
            if (string.IsNullOrEmpty(session) || expenditures == null || !expenditures.Any())
                return 0.0;

            return expenditures
                .Where(k => k.SessionYear != null && k.SessionYear.Name == session)
                .Sum(item => item.Amount);
        }
    }
}
