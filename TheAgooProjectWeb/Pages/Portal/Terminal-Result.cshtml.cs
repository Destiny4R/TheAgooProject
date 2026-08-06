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

namespace TheAgooProjectWeb.Pages.Portal
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer},{SD.IsStudent}")]
    public class Terminal_ResultModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public int NoInClass { get; set; } = 0;
        public int Position { get; set; } = 0;
        public double ToTalScore { get; set; } = 0;
        public StudentsData Studentsdata { get; set; } = null!;
        public TermRegistration termReg { get; set; } = null!;
        public GeneralClassTable _generalClass { get; set; } = null!;
        public string signature { get; set; } = string.Empty;
        public RemarkPosition _remarkPosition { get; set; } = null!;
        public bool IsSenior { get; set; } = false;

        // Pre-computed summary metrics for clean Razor binding & optimal performance
        public int NoOfSubjects { get; set; } = 0;
        public double ObtainableMarks { get; set; } = 0;
        public double TotalScoreObtained { get; set; } = 0;
        public double TerminalAverage { get; set; } = 0;
        public string TeacherRemark { get; set; } = string.Empty;
        public string PrincipalRemark { get; set; } = string.Empty;

        public IList<CollectData> CollectDatas { get; set; } = new List<CollectData>();
        public IList<HighLowScore> HighLowPosition { get; set; } = new List<HighLowScore>();
        public IList<HighLowScore> HighPosition { get; set; } = new List<HighLowScore>();

        public Terminal_ResultModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int termregid)
        {
            if (termregid <= 0)
            {
                TempData["error"] = "Invalid result request. Please try again.";
                return RedirectToPage("index");
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            termReg = await dbContext.TermRegistrations.AsNoTracking()
                .Include(t => t.StudentsData)
                .Include(t => t.SessionYear)
                .Include(t => t.SubClasses)
                .Include(t => t.RemarkPositions)
                .Include(t => t.StudentRatings)
                .Include(t => t.Schoolclasses)
                .FirstOrDefaultAsync(t => t.Id == termregid);

            if (termReg == null)
            {
                TempData["error"] = "No result was found for the specified registration.";
                return RedirectToPage("index");
            }

            bool isAdminOrExamOfficer = User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_ExamOfficer);

            // Students can only view their own terminal result
            if (!isAdminOrExamOfficer)
            {
                var currentStudent = await dbContext.StudentTable.AsNoTracking().FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);
                if (currentStudent == null || termReg.StudentId != currentStudent.Id)
                {
                    TempData["error"] = "Sorry! You are not authorized to view another student's result.";
                    return RedirectToPage("index");
                }
            }

            var admin = (await userManager.GetUsersInRoleAsync(SD.Role_Admin)).FirstOrDefault();
            if (admin != null)
            {
                var settings = await dbContext.Settings.AsNoTracking().FirstOrDefaultAsync(k => k.ApplicationUserId == admin.Id);
                if (settings == null)
                {
                    TempData["error"] = "Admin is yet to configure system settings. Please contact your system administrator.";
                    return RedirectToPage("index");
                }
                signature = settings.PrincipalSignature ?? string.Empty;

                // Fee payment restriction check for student role when result printing is disabled
                if (!isAdminOrExamOfficer && !settings.CanPrintResult)
                {
                    var fees = await dbContext.PaymentReports.AsNoTracking()
                        .FirstOrDefaultAsync(k => k.TermRegId == termReg.Id);
                    if (fees == null || fees.Status != SD.Fees_Status_Completed)
                    {
                        TempData["error"] = "Our system has detected an incomplete fee payment. Please visit fee history for details.";
                        return RedirectToPage("index");
                    }
                }
            }

            var session = termReg.SessionYearId;
            var classes = termReg.ClassesInSchoolId;
            var subclass = termReg.SubClassId;
            var term = termReg.Term;
            var studentid = termReg.StudentId;

            Studentsdata = termReg.StudentsData;

            // Fetch class-wide results in a single read-only query
            var classResults = await dbContext.ResultTable.AsNoTracking()
                .Include(k => k.Subjects)
                .Include(k => k.Termregistration)
                .Where(s => s.Termregistration.SessionYearId == session 
                         && s.Termregistration.ClassesInSchoolId == classes 
                         && s.Termregistration.SubClassId == subclass 
                         && s.Termregistration.Term == term)
                .ToListAsync();

            if (!classResults.Any())
            {
                TempData["error"] = "No results found for the selection, are you missing something?";
                return RedirectToPage("index");
            }

            NoInClass = classResults.Select(y => y.Termregistration.StudentId).Distinct().Count();

            _generalClass = await dbContext.GeneralClassTables.AsNoTracking()
                .Include(s => s.SessionYear)
                .Include(s => s.Classcchool)
                .Include(s => s.SubClasses)
                .FirstOrDefaultAsync(k => k.SessionYearId == session 
                                       && k.Term == term 
                                       && k.ClassId == classes 
                                       && k.SubClassId == subclass);

            if (_generalClass == null)
            {
                TempData["error"] = "Result still under computation. General class information not available.";
                return RedirectToPage("index");
            }

            IsSenior = termReg.Schoolclasses?.Elective ?? false;
            _remarkPosition = termReg.RemarkPositions;

            if (_remarkPosition == null || !_remarkPosition.R_Status || !_remarkPosition.P_Status)
            {
                TempData["error"] = "Result still under computation. Check back later.";
                return RedirectToPage("index");
            }

            // Single-pass calculation for class student totals and rankings
            HighPosition = classResults
                .GroupBy(y => y.Termregistration.StudentId)
                .Select(g =>
                {
                    int sId = g.Key;
                    double total = g.Sum(x => x.Total ?? 0.0);
                    int count = g.Count();
                    double avg = count > 0 ? Math.Round(total / count, 2) : 0.0;
                    return new HighLowScore { StudentId = sId, Score = total, Average = avg };
                })
                .ToList();

            var rankedClassPositions = HighPosition.Rank(p => p.Average, (p, rank) => new HighLowScore
            {
                Average = p.Average,
                Score = p.Score,
                StudentId = p.StudentId,
                Rank = rank
            }).ToList();

            var currentStudentClassPos = rankedClassPositions.FirstOrDefault(m => m.StudentId == studentid);
            Position = currentStudentClassPos?.Rank ?? 0;
            ToTalScore = currentStudentClassPos?.Score ?? 0.0;

            // Single-pass pre-aggregation for per-subject statistics & rankings
            var subjectStats = classResults
                .GroupBy(j => j.SubjectId)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        double highest = g.Max(k => k.Total ?? 0.0);
                        double lowest = g.Min(k => k.Total ?? 0.0);
                        int totalCount = g.Count();
                        double avg = totalCount > 0 ? Math.Round(g.Sum(x => x.Total ?? 0.0) / totalCount, 2) : 0.0;

                        var subjectRanks = g.Select(p => new Averages
                        {
                            StudentId = p.Termregistration.StudentId,
                            SubjId = p.SubjectId,
                            Score = p.Total ?? 0.0
                        })
                        .Rank(p => p.Score, (p, rank) => new Averages
                        {
                            StudentId = p.StudentId,
                            SubjId = p.SubjId,
                            Score = p.Score,
                            Rank = rank
                        })
                        .ToDictionary(r => r.StudentId, r => r.Rank);

                        return (Highest: highest, Lowest: lowest, Average: avg, Ranks: subjectRanks);
                    }
                );

            // Populate subject data for current student
            var studentResults = classResults.Where(k => k.Termregistration.StudentId == studentid).ToList();
            CollectDatas = new List<CollectData>();

            foreach (var item in studentResults)
            {
                int subjectId = item.SubjectId;
                double totalScore = item.Total ?? 0.0;

                double highest = 0.0, lowest = 0.0, subjAvg = 0.0;
                int subjPosition = 0;

                if (subjectStats.TryGetValue(subjectId, out var stats))
                {
                    highest = stats.Highest;
                    lowest = stats.Lowest;
                    subjAvg = stats.Average;
                    if (stats.Ranks.TryGetValue(studentid, out int r))
                    {
                        subjPosition = r;
                    }
                }

                CollectDatas.Add(new CollectData
                {
                    SubjectId = subjectId,
                    SujectName = item.Subjects?.Name ?? "Unknown Subject",
                    Assgmnt = item.Assignment,
                    Test = item.Test,
                    CWork = item.ClassWork,
                    project = item.Project,
                    Exams = item.Examination,
                    TotalScores = totalScore,
                    Average = subjAvg,
                    Highest = highest,
                    Lowest = lowest,
                    Position = subjPosition,
                    Grade = SD.Grade(totalScore),
                    Remark = SD.Remark(totalScore)
                });
            }

            // Compute summary metrics for Razor View
            NoOfSubjects = CollectDatas.Count;
            ObtainableMarks = NoOfSubjects * 100;
            TotalScoreObtained = ToTalScore;
            TerminalAverage = NoOfSubjects > 0 ? Math.Round(ToTalScore / NoOfSubjects, 2) : 0.0;
            TeacherRemark = SD.GetPerformanceComment(TerminalAverage);
            PrincipalRemark = SD.GetComment(TerminalAverage);

            return Page();
        }

        public class CollectData
        {
            public string SujectName { get; set; } = string.Empty;
            public int SubjectId { get; set; }
            public double? Assgmnt { get; set; }
            public double? Test { get; set; }
            public double? CWork { get; set; }
            public double? project { get; set; }
            public double? Exams { get; set; }
            public double? TotalScores { get; set; }
            public double Average { get; set; }
            public double Highest { get; set; }
            public double Lowest { get; set; }
            public int Position { get; set; }
            public string Grade { get; set; } = string.Empty;
            public string Remark { get; set; } = string.Empty;
        }

        public class HighLowScore
        {
            public int StudentId { get; set; }
            public double Average { get; set; }
            public double Score { get; set; }
            public int Rank { get; set; }
        }

        public class Averages
        {
            public int StudentId { get; set; }
            public int SubjId { get; set; }
            public double Score { get; set; }
            public int Rank { get; set; }
        }
    }
}
