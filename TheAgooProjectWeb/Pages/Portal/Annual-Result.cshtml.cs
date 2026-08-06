using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.student
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer},{SD.IsStudent}")]
    public class Annual_ResultModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public string signature { get; set; } = string.Empty;
        public StudentsData Studentsdata { get; set; } = null!;
        public GeneralClassTable _generalClass { get; set; } = null!;
        public RemarkPosition _remarkPosition { get; set; } = null!;

        public int termly { get; set; } = 0;
        public int NoinClass { get; set; } = 0;
        public int PositionInClass { get; set; } = 0;
        public int activeTerms { get; set; } = 0;

        // Computed summary statistics for high performance and clean Razor view binding
        public double ObtainableMarks { get; set; } = 0;
        public double ObtainedMarks { get; set; } = 0;
        public double FirstTermAverage { get; set; } = 0;
        public double SecondTermAverage { get; set; } = 0;
        public double ThirdTermAverage { get; set; } = 0;
        public double CumulativeAverage { get; set; } = 0;
        public double AnnualAverage { get; set; } = 0;
        public bool IsQualified { get; set; } = false;
        public string TeacherRemark { get; set; } = string.Empty;
        public string PrincipalRemark { get; set; } = string.Empty;

        public IList<CollectData> CollectDatas { get; set; } = new List<CollectData>();
        public IList<HighLowScore> HighLowScores { get; set; } = new List<HighLowScore>();
        public IList<HighLowScore> HighPosition { get; set; } = new List<HighLowScore>();

        public Annual_ResultModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int session, int studentid)
        {
            if (session <= 0 || studentid <= 0)
            {
                TempData["error"] = "Sorry! this request cannot be processed at this time, required parameter(s) missing.";
                return RedirectToPage("index");
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            // Look up requested student record by studentid
            var student = await dbContext.StudentTable.AsNoTracking().FirstOrDefaultAsync(s => s.Id == studentid);
            if (student == null)
            {
                TempData["error"] = "We could not find the student record. Please contact the school administrator.";
                return RedirectToPage("index");
            }

            bool isAdminOrExamOfficer = User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_ExamOfficer);

            // If the user is a student, ensure they are accessing only their own student record
            if (!isAdminOrExamOfficer)
            {
                if (student.ApplicationUserId != user.Id)
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
                    TempData["error"] = "Admin is yet to configure system settings. Please contact your administrator.";
                    return RedirectToPage("index");
                }
                signature = settings.PrincipalSignature ?? string.Empty;

                // Fee payment check applies to student role when printing is restricted
                if (!isAdminOrExamOfficer && !settings.CanPrintResult)
                {
                    var payments = await dbContext.Payments.AsNoTracking()
                        .Include(p => p.Termregistrations)
                        .Where(p => p.Termregistrations.StudentId == studentid && p.Termregistrations.SessionYearId == session)
                        .Select(p => new { p.Termregistrations.Term, p.Status })
                        .ToListAsync();

                    int unpaidTerms = SD.Terms.Count(term => !payments.Any(p => p.Term == term && p.Status == SD.Fees_Status_Completed));
                    if (unpaidTerms > 0)
                    {
                        TempData["error"] = "Our system has detected an incomplete fee payment. Please visit fee history for details.";
                        return RedirectToPage("index");
                    }
                }
            }

            var studentTermRegs = await dbContext.TermRegistrations.AsNoTracking()
                .Where(h => h.SessionYearId == session && h.StudentId == studentid)
                .ToListAsync();

            if (!studentTermRegs.Any())
            {
                TempData["error"] = "Student registration for the specified session was not found.";
                return RedirectToPage("index");
            }

            var termReg = studentTermRegs.FirstOrDefault();

            // Fetch class-wide results in a single read-only query
            var classResults = await dbContext.ResultTable.AsNoTracking()
                .Include(k => k.Subjects)
                .Include(k => k.Termregistration)
                .Where(s => s.Termregistration.SessionYearId == session 
                         && s.Termregistration.ClassesInSchoolId == termReg.ClassesInSchoolId 
                         && s.Termregistration.SubClassId == termReg.SubClassId)
                .ToListAsync();

            if (!classResults.Any())
            {
                TempData["error"] = "No academic results available for the selected class and session.";
                return RedirectToPage("index");
            }

            Studentsdata = student;

            // Single query to retrieve general class information (prefer Third term -> Second term -> First term)
            var generalClassList = await dbContext.GeneralClassTables.AsNoTracking()
                .Include(s => s.SessionYear)
                .Include(s => s.Classcchool)
                .Include(s => s.SubClasses)
                .Where(k => k.SessionYearId == termReg.SessionYearId 
                         && k.ClassId == termReg.ClassesInSchoolId 
                         && k.SubClassId == termReg.SubClassId)
                .ToListAsync();

            _generalClass = generalClassList.FirstOrDefault(k => k.Term == SD.Third)
                         ?? generalClassList.FirstOrDefault(k => k.Term == SD.Second)
                         ?? generalClassList.FirstOrDefault(k => k.Term == SD.First);

            if (_generalClass == null)
            {
                TempData["error"] = "Result unavailable. General class information for this session/class has not been created.";
                return RedirectToPage("index");
            }

            var studentFirstResult = classResults.FirstOrDefault(k => k.Termregistration.StudentId == studentid);
            if (studentFirstResult != null)
            {
                _remarkPosition = await dbContext.RemarkPositions.AsNoTracking()
                    .FirstOrDefaultAsync(k => k.TermRegId == studentFirstResult.TermRegId);
            }

            // Count distinct students in class and terms registered by this student
            NoinClass = classResults.Select(k => k.Termregistration.StudentId).Distinct().Count();
            activeTerms = classResults.Select(h => h.Termregistration.Term).Distinct().Count();

            int studentRegisteredTermCount = studentTermRegs.Select(h => h.Term).Distinct().Count();
            if (studentRegisteredTermCount <= 0)
            {
                studentRegisteredTermCount = classResults
                    .Where(m => m.Termregistration.StudentId == studentid)
                    .Select(h => h.Termregistration.Term)
                    .Distinct()
                    .Count();
            }
            termly = studentRegisteredTermCount > 0 ? studentRegisteredTermCount : 1;

            // Pre-calculate per-subject student totals across terms for efficient ranking
            // Map: SubjectId -> StudentId -> Total Score in Subject (double)
            var subjectStudentTotals = classResults
                .GroupBy(r => r.SubjectId)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(r => r.Termregistration.StudentId)
                          .ToDictionary(
                              sg => sg.Key,
                              sg => sg.Sum(x => x.Total ?? 0.0)
                          )
                );

            // Pre-calculate subject rankings and statistics
            var subjectStats = new Dictionary<int, (double Highest, double Lowest, Dictionary<int, int> Ranks)>();

            foreach (var kvp in subjectStudentTotals)
            {
                int subjectId = kvp.Key;
                var studentScores = kvp.Value;

                double highestScore = studentScores.Values.Any() ? studentScores.Values.Max() : 0.0;
                double lowestScore = studentScores.Values.Any() ? studentScores.Values.Min() : 0.0;

                // Rank students for this subject
                var highLowList = studentScores.Select(s => new HighLowScore { StudentId = s.Key, Score = s.Value }).ToList();
                var rankedList = highLowList.Rank(p => p.Score, (p, rank) => new HighLowScore { StudentId = p.StudentId, Score = p.Score, Rank = rank }).ToList();
                var rankDict = rankedList.ToDictionary(r => r.StudentId, r => r.Rank);

                subjectStats[subjectId] = (highestScore, lowestScore, rankDict);
            }

            // Populate subject data for current student
            var studentResults = classResults.Where(m => m.Termregistration.StudentId == studentid).ToList();
            CollectDatas = new List<CollectData>();

            foreach (var subjectGroup in studentResults.GroupBy(n => n.SubjectId))
            {
                int subjectId = subjectGroup.Key;
                var firstEntry = subjectGroup.FirstOrDefault();
                string subjectName = firstEntry?.Subjects?.Name ?? "Unknown Subject";

                double firstTerm = subjectGroup.FirstOrDefault(k => k.Termregistration.Term == SD.First)?.Total ?? 0.0;
                double secondTerm = subjectGroup.FirstOrDefault(k => k.Termregistration.Term == SD.Second)?.Total ?? 0.0;
                double thirdTerm = subjectGroup.FirstOrDefault(k => k.Termregistration.Term == SD.Third)?.Total ?? 0.0;
                double totalScore = firstTerm + secondTerm + thirdTerm;

                double yearlyAv = Math.Round(totalScore / termly, 2);

                double highest = 0.0, lowest = 0.0;
                int subjectRank = 0;

                if (subjectStats.TryGetValue(subjectId, out var stats))
                {
                    highest = stats.Highest;
                    lowest = stats.Lowest;
                    if (stats.Ranks.TryGetValue(studentid, out int r))
                    {
                        subjectRank = r;
                    }
                }

                string grade = SD.Annual_Calculate_Grade2(totalScore, termly);
                string remark = SD.Annual_Calculate_Remark2(totalScore, termly);

                CollectDatas.Add(new CollectData
                {
                    SubjectId = subjectId,
                    SujectName = subjectName,
                    First = firstTerm,
                    Second = secondTerm,
                    Third = thirdTerm,
                    TotalScores = totalScore,
                    YearlyAv = yearlyAv,
                    Highest = highest,
                    Lowest = lowest,
                    Position = subjectRank,
                    Grade = grade,
                    Remark = remark
                });
            }

            // Calculate overall class positions and averages
            HighPosition = new List<HighLowScore>();
            foreach (var studentGroup in classResults.GroupBy(m => m.Termregistration.StudentId))
            {
                int sId = studentGroup.Key;
                double totalScore = studentGroup.Sum(x => x.Total ?? 0.0);
                int count = studentGroup.Count();
                double avg = count > 0 ? Math.Round(totalScore / count, 2) : 0.0;

                HighPosition.Add(new HighLowScore
                {
                    StudentId = sId,
                    Score = totalScore,
                    Average = avg
                });
            }

            HighPosition = HighPosition.Rank(p => p.Average, (p, rank) => new HighLowScore
            {
                Average = p.Average,
                Score = p.Score,
                StudentId = p.StudentId,
                Rank = rank
            }).ToList();

            var currentStudentRank = HighPosition.FirstOrDefault(k => k.StudentId == studentid);
            PositionInClass = currentStudentRank?.Rank ?? 0;

            // Compute summary statistics for Razor View
            int subjectCount = CollectDatas.Count;
            ObtainableMarks = termly * subjectCount * 100;
            ObtainedMarks = CollectDatas.Sum(d => d.TotalScores);

            if (subjectCount > 0)
            {
                FirstTermAverage = Math.Round(CollectDatas.Average(k => k.First), 2);
                SecondTermAverage = Math.Round(CollectDatas.Average(k => k.Second), 2);
                ThirdTermAverage = Math.Round(CollectDatas.Average(k => k.Third), 2);
            }

            CumulativeAverage = Math.Round(FirstTermAverage + SecondTermAverage + ThirdTermAverage, 2);
            AnnualAverage = Math.Round(CumulativeAverage / (termly > 0 ? termly : 1), 2);

            IsQualified = AnnualAverage >= 40;
            TeacherRemark = SD.GetPerformanceComment(AnnualAverage);
            PrincipalRemark = SD.GetComment(AnnualAverage);

            return Page();
        }

        public class CollectData
        {
            public string SujectName { get; set; } = string.Empty;
            public int SubjectId { get; set; }
            public double First { get; set; }
            public double Second { get; set; }
            public double Third { get; set; }
            public double TotalScores { get; set; }
            public double YearlyAv { get; set; }
            public double Highest { get; set; }
            public double Lowest { get; set; }
            public int Position { get; set; }
            public string Grade { get; set; } = string.Empty;
            public string Remark { get; set; } = string.Empty;
        }

        public class HighLowScore
        {
            public int StudentId { get; set; }
            public double Score { get; set; }
            public double Average { get; set; }
            public int Rank { get; set; }
        }
    }
}
