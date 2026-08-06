using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.student
{
    [Authorize(Roles = SD.IsStudent)]
    public class indexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public indexModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public ViewSelectModel ViewSelectModel { get; set; }
        public bool HasRecords { get; set; }
        public string StudentFullName { get; set; } = string.Empty;
        public string StudentRegNo { get; set; } = string.Empty;

        public string SessionLabelsJson { get; set; } = "[]";
        public string SessionScoresJson { get; set; } = "[]";
        public string TermFirstDataJson { get; set; } = "[]";
        public string TermSecondDataJson { get; set; } = "[]";
        public string TermThirdDataJson { get; set; } = "[]";

        public IList<string> Outstanding { get; set; } = new List<string>();

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }

        [BindProperty]
        public int AnnualSessionName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            var student = await dbContext.StudentTable.AsNoTracking().FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);
            if (student == null)
            {
                TempData["error"] = "We could not find your student record. Please contact the school administrator.";
                return Page();
            }

            StudentFullName = $"{student.SurName} {student.OtherName} {student.FirstName}".Replace("  ", " ");
            StudentRegNo = student.StudentRegNo ?? user.UserName;

            getViewSelect(student.Id);

            var termRegs = await dbContext.TermRegistrations
                .AsNoTracking()
                .Include(t => t.SessionYear)
                .Include(t => t.Schoolclasses)
                .Include(t => t.SubClasses)
                .Where(t => t.StudentId == student.Id)
                .OrderBy(t => t.SessionYear.Name)
                .ToListAsync();

            if (!termRegs.Any())
            {
                HasRecords = false;
                return Page();
            }
            HasRecords = true;

            var termRegIds = termRegs.Select(t => t.Id).ToList();
            var results = await dbContext.ResultTable
                .AsNoTracking()
                .Include(r => r.Termregistration)
                .Where(r => termRegIds.Contains(r.TermRegId))
                .ToListAsync();

            BuildSessionChart(termRegs, results);
            BuildTermChart(termRegs, results);
            BuildOutstanding(termRegs);

            return Page();
        }

        public async Task<IActionResult> OnPostPrintTerminalAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            var student = await dbContext.StudentTable.AsNoTracking().FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);
            if (student == null)
            {
                TempData["error"] = "We could not find your student record. Please contact the school administrator.";
                return RedirectToPage();
            }

            getViewSelect(student.Id);

            if (CTSSClass == null || CTSSClass.SessionYear <= 0 || string.IsNullOrWhiteSpace(CTSSClass.Term)
                || CTSSClass.Classes <= 0)
            {
                TempData["error"] = "Please select the Session, Term, Class and Sub-Class before printing.";
                return RedirectToPage();
            }

            var termReg = await dbContext.TermRegistrations
                .Include(t => t.SessionYear)
                .Include(t => t.Schoolclasses)
                .Include(t => t.SubClasses)
                .FirstOrDefaultAsync(t =>
                    t.StudentId == student.Id &&
                    t.SessionYearId == CTSSClass.SessionYear &&
                    t.Term == CTSSClass.Term &&
                    t.ClassesInSchoolId == CTSSClass.Classes);

            if (termReg == null)
            {
                TempData["error"] = "No registration was found for the selected session, term, class and sub-class.";
                return RedirectToPage();
            }

            var admin = (await userManager.GetUsersInRoleAsync(SD.Role_Admin)).FirstOrDefault();
            bool canPrint = false;
            if (admin != null)
            {
                var settings = await dbContext.Settings.AsNoTracking().FirstOrDefaultAsync(s => s.ApplicationUserId == admin.Id);
                canPrint = settings?.CanPrintResult ?? false;
            }

            if (!canPrint)
            {
                var payment = await dbContext.PaymentReports.AsNoTracking().FirstOrDefaultAsync(p => p.TermRegId == termReg.Id);
                if (payment == null || payment.Status != SD.Fees_Status_Completed)
                {
                    TempData["error"] =
                        $"Your school fees for <strong>{termReg.SessionYear.Name} {termReg.Term} Term</strong> have not been fully paid. " +
                        "Please complete your payment before accessing your result. If you believe this is an error, kindly contact the school administrator.";
                    return RedirectToPage();
                }
            }

            return RedirectToPage("/Portal/Terminal-Result", new { termregid = termReg.Id });
        }

        public async Task<IActionResult> OnPostPrintAnnualAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            var student = await dbContext.StudentTable.AsNoTracking().FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);
            if (student == null)
            {
                TempData["error"] = "We could not find your student record. Please contact the school administrator.";
                return RedirectToPage();
            }

            getViewSelect(student.Id);

            if (AnnualSessionName <= 0)
            {
                TempData["error"] = "Please select the academic session you want to print.";
                return RedirectToPage();
            }

            var session = await dbContext.SessionYears.AsNoTracking().FirstOrDefaultAsync(s => s.Id == AnnualSessionName);
            if (session == null)
            {
                TempData["error"] = "The selected academic session could not be found.";
                return RedirectToPage();
            }

            var admin = (await userManager.GetUsersInRoleAsync(SD.Role_Admin)).FirstOrDefault();
            bool canPrint = false;
            if (admin != null)
            {
                var settings = await dbContext.Settings.AsNoTracking().FirstOrDefaultAsync(s => s.ApplicationUserId == admin.Id);
                canPrint = settings?.CanPrintResult ?? false;
            }

            if (!canPrint)
            {
                var payments = await dbContext.Payments.AsNoTracking()
                    .Include(p => p.Termregistrations)
                    .Where(p => p.Termregistrations.StudentId == student.Id && p.Termregistrations.SessionYearId == AnnualSessionName)
                    .Select(p => new { p.Termregistrations.Term, p.Status })
                    .ToListAsync();

                int unpaidTerms = SD.Terms.Count(term => !payments.Any(p => p.Term == term && p.Status == SD.Fees_Status_Completed));
                if (unpaidTerms > 0)
                {
                    var termText = SD.MoneyToWords(unpaidTerms).Replace(" Naira Only.", string.Empty).ToLower();
                    TempData["error"] = $"We noticed you have outstanding school fees for {termText} term{(unpaidTerms > 1 ? "s" : "")}. Please check your payment history.";
                    return RedirectToPage();
                }
            }

            return RedirectToPage("/Portal/Annual-Result", new { session = session.Id, studentid = student.Id });
        }

        public async Task<IActionResult> OnGetClassOptionsAsync(int sessionId, string term)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return new JsonResult(new object[0]);

            var student = await dbContext.StudentTable.AsNoTracking().FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);
            if (student == null) return new JsonResult(new object[0]);

            var classes = await dbContext.TermRegistrations
                .AsNoTracking()
                .Include(t => t.Schoolclasses)
                .Include(t => t.SubClasses)
                .Where(t =>
                    t.StudentId == student.Id &&
                    t.SessionYearId == sessionId &&
                    t.Term == term)
                .Select(t => new
                {
                    classId = t.ClassesInSchoolId,
                    className = t.Schoolclasses.Name,
                    subClassId = t.SubClassId,
                    subClassName = t.SubClasses.Name
                })
                .Distinct()
                .ToListAsync();

            return new JsonResult(classes);
        }

        private void BuildOutstanding(List<TermRegistration> termRegs)
        {
            Outstanding = new List<string>();
            foreach (var term in termRegs)
            {
                var fees = dbContext.TermLyFeesPays.FirstOrDefault(o =>
                    o.Term == term.Term &&
                    o.SessionYearId == term.SessionYearId &&
                    o.ClassesInSchoolId == term.ClassesInSchoolId);

                var cummulative = dbContext.PaymentReports.FirstOrDefault(k => k.TermRegId == term.Id);
                double outstanding = 0;

                if (fees != null)
                {
                    outstanding = cummulative == null
                        ? fees.Amount
                        : fees.Amount - cummulative.AccummulatedPay;
                }

                if (outstanding > 0)
                {
                    Outstanding.Add($"<strong>{SD.ToNaira(outstanding)}</strong> for {term.SessionYear.Name} - {term.Term} Term - {term.Schoolclasses.Name} - {term.SubClasses.Name}");
                }
            }
        }

        private void BuildSessionChart(List<TermRegistration> termRegs, List<ResultTable> results)
        {
            var sessions = termRegs
                .GroupBy(t => new { t.SessionYearId, t.SessionYear.Name })
                .OrderBy(g => g.Key.Name)
                .Select(g => new
                {
                    Label = g.Key.Name,
                    TermRegIds = g.Select(t => t.Id).ToList()
                })
                .ToList();

            var labels = new List<string>();
            var scores = new List<double>();

            foreach (var session in sessions)
            {
                labels.Add(session.Label);
                scores.Add(results.Where(r => session.TermRegIds.Contains(r.TermRegId)).Sum(r => r.Total ?? 0));
            }

            var opts = new JsonSerializerOptions { PropertyNamingPolicy = null };
            SessionLabelsJson = JsonSerializer.Serialize(labels, opts);
            SessionScoresJson = JsonSerializer.Serialize(scores, opts);
        }

        private void BuildTermChart(List<TermRegistration> termRegs, List<ResultTable> results)
        {
            var sessions = termRegs
                .GroupBy(t => new { t.SessionYearId, t.SessionYear.Name })
                .OrderBy(g => g.Key.Name)
                .Select(g => new
                {
                    Label = g.Key.Name,
                    Id = g.Key.SessionYearId,
                    TermRegIds = g.Select(t => t.Id).ToList()
                })
                .ToList();

            var firstScores = new List<double>();
            var secondScores = new List<double>();
            var thirdScores = new List<double>();

            foreach (var session in sessions)
            {
                var sessionRegs = termRegs.Where(t => t.SessionYearId == session.Id).ToList();
                var firstIds = sessionRegs.Where(t => t.Term == "First").Select(t => t.Id).ToList();
                var secondIds = sessionRegs.Where(t => t.Term == "Second").Select(t => t.Id).ToList();
                var thirdIds = sessionRegs.Where(t => t.Term == "Third").Select(t => t.Id).ToList();

                firstScores.Add(results.Where(r => firstIds.Contains(r.TermRegId)).Sum(r => r.Total ?? 0));
                secondScores.Add(results.Where(r => secondIds.Contains(r.TermRegId)).Sum(r => r.Total ?? 0));
                thirdScores.Add(results.Where(r => thirdIds.Contains(r.TermRegId)).Sum(r => r.Total ?? 0));
            }

            var opts = new JsonSerializerOptions { PropertyNamingPolicy = null };
            SessionLabelsJson = JsonSerializer.Serialize(sessions.Select(s => s.Label).ToList(), opts);
            TermFirstDataJson = JsonSerializer.Serialize(firstScores, opts);
            TermSecondDataJson = JsonSerializer.Serialize(secondScores, opts);
            TermThirdDataJson = JsonSerializer.Serialize(thirdScores, opts);
        }

        private void getViewSelect(int studentId)
        {
            var registrations = dbContext.TermRegistrations
                .Include(t => t.Schoolclasses)
                .Include(t => t.SubClasses)
                .Include(t => t.SessionYear)
                .Where(t => t.StudentId == studentId)
                .ToList();

            var sessions = registrations
                .GroupBy(t => t.SessionYearId)
                .Select(g => new SelectListItem
                {
                    Text = g.First().SessionYear.Name,
                    Value = g.Key.ToString()
                })
                .OrderByDescending(k => k.Text)
                .ToList();

            var classes = registrations
                .GroupBy(t => t.ClassesInSchoolId)
                .Select(g => new SelectListItem
                {
                    Text = g.First().Schoolclasses.Name,
                    Value = g.Key.ToString()
                })
                .ToList();

            var subClasses = registrations
                .GroupBy(t => t.SubClassId)
                .Select(g => new SelectListItem
                {
                    Text = g.First().SubClasses.Name,
                    Value = g.Key.ToString()
                })
                .ToList();

            ViewSelectModel = new()
            {
                SessionYearz = sessions,
                Class = classes,
                SubClass = subClasses
            };
        }
    }
}
