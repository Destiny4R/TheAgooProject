using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Portal
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer}")]
    public class AnnualMasterModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AnnualMasterModel> _logger;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; } = new();

        public ViewSelectModel ViewSelectModel { get; set; } = new();

        /// <summary>Ordered list of subjects offered by this class across the session.</summary>
        public IList<SubjectInfo> Subjects { get; set; } = new List<SubjectInfo>();

        /// <summary>One row per registered student, pre-aggregated for the view.</summary>
        public IList<StudentRow> StudentRows { get; set; } = new List<StudentRow>();

        /// <summary>Per-subject footer statistics (termly/annual averages, highest/lowest).</summary>
        public IList<SubjectSummary> SubjectSummaries { get; set; } = new List<SubjectSummary>();

        public string BannerSession { get; set; } = string.Empty;
        public string BannerClass { get; set; } = string.Empty;
        public string BannerSubClass { get; set; } = string.Empty;
        public string BannerTerms { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public int SubjectCount { get; set; }
        public int ActiveTermCount { get; set; }

        public string ExcelFileName { get; set; } = "annual_master_sheet";

        public AnnualMasterModel(ApplicationDbContext dbContext, ILogger<AnnualMasterModel> logger)
        {
            _db = dbContext;
            _logger = logger;
        }

        public void OnGet()
        {
            LoadFilterOptions();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoadFilterOptions();

            if (CTSSClass.SessionYear <= 0 || CTSSClass.Classes <= 0 || CTSSClass.Subclass <= 0)
            {
                TempData["error"] = "Please select the Session/Year, Class and Sub-Class, then try again.";
                return Page();
            }

            try
            {
                // 1. Load ALL results for the class across the whole session in a single query.
                var results = await _db.ResultTable
                    .AsNoTracking()
                    .Include(r => r.Subjects)
                    .Include(r => r.Termregistration)
                        .ThenInclude(t => t.StudentsData)
                    .Include(r => r.Termregistration)
                        .ThenInclude(t => t.SessionYear)
                    .Include(r => r.Termregistration)
                        .ThenInclude(t => t.Schoolclasses)
                    .Include(r => r.Termregistration)
                        .ThenInclude(t => t.SubClasses)
                    .Where(r =>
                        r.Termregistration.SessionYearId == CTSSClass.SessionYear &&
                        r.Termregistration.ClassesInSchoolId == CTSSClass.Classes &&
                        r.Termregistration.SubClassId == CTSSClass.Subclass)
                    .ToListAsync();

                if (results.Count == 0)
                {
                    TempData["error"] = "No results were found for the selected combination. Ensure results have been uploaded for this class across the session.";
                    return Page();
                }

                // 2. Banner labels.
                var firstResult = results[0];
                BannerSession = firstResult.Termregistration.SessionYear.Name;
                BannerClass = firstResult.Termregistration.Schoolclasses.Name;
                BannerSubClass = firstResult.Termregistration.SubClasses.Name;
                BannerTerms = string.Join(", ", results.Select(r => r.Termregistration.Term).Distinct());

                // 3. Ordered subject list (distinct, sorted by name).
                Subjects = results
                    .GroupBy(r => r.SubjectId)
                    .OrderBy(g => g.First().Subjects.Name)
                    .Select(g => new SubjectInfo
                    {
                        SubjectId = g.Key,
                        SubjectName = g.First().Subjects.Name
                    })
                    .ToList();

                ActiveTermCount = results.Select(r => r.Termregistration.Term).Distinct().Count();
                SubjectCount = Subjects.Count;

                // 4. Roster: every student registered in this class/session, even without scores.
                var registrations = await _db.TermRegistrations
                    .AsNoTracking()
                    .Include(t => t.StudentsData)
                    .Where(t =>
                        t.SessionYearId == CTSSClass.SessionYear &&
                        t.ClassesInSchoolId == CTSSClass.Classes &&
                        t.SubClassId == CTSSClass.Subclass)
                    .ToListAsync();

                // 5. Lookup: (StudentId, SubjectId, Term) -> total score.
                var totalByKey = results
                    .GroupBy(r => (r.Termregistration.StudentId, r.SubjectId, r.Termregistration.Term))
                    .ToDictionary(g => g.Key, g => g.Sum(r => r.Total ?? 0));

                // 6. Annual total per student + a single consistent annual average (% of achievable max).
                var studentTotals = results
                    .GroupBy(r => r.Termregistration.StudentId)
                    .ToDictionary(g => g.Key, g => g.Sum(r => r.Total ?? 0));

                double maxScore = Subjects.Count * ActiveTermCount * 100.0;
                if (maxScore <= 0)
                {
                    maxScore = 1;
                }

                // 7. Competition ranking by annual average (same value shown in the sheet).
                var ranks = studentTotals
                    .Select(kv => new
                    {
                        StudentId = kv.Key,
                        Average = Math.Round(kv.Value / maxScore * 100.0, 2)
                    })
                    .ToList()
                    .Rank(p => p.Average, (p, rank) => new { p.StudentId, Rank = rank })
                    .ToDictionary(x => x.StudentId, x => x.Rank);

                // 8. Build student rows (ordered by full name).
                int sn = 0;
                var rows = new List<StudentRow>();
                foreach (var group in registrations
                             .GroupBy(t => t.StudentId)
                             .OrderBy(g => BuildFullName(g.First().StudentsData)))
                {
                    sn++;
                    var studentId = group.Key;
                    var student = group.First().StudentsData;

                    var row = new StudentRow
                    {
                        SerialNo = sn,
                        StudentId = studentId,
                        AdmissionNo = student.StudentRegNo ?? "—",
                        FullName = BuildFullName(student),
                        Gender = student.Gender ?? string.Empty
                    };

                    foreach (var sub in Subjects)
                    {
                        bool hasFirst = totalByKey.TryGetValue((studentId, sub.SubjectId, SD.First), out var firstScore);
                        bool hasSecond = totalByKey.TryGetValue((studentId, sub.SubjectId, SD.Second), out var secondScore);
                        bool hasThird = totalByKey.TryGetValue((studentId, sub.SubjectId, SD.Third), out var thirdScore);

                        if (hasFirst || hasSecond || hasThird)
                        {
                            row.SubjectTotals[sub.SubjectId] = new SubjectTotals
                            {
                                First = hasFirst ? firstScore : 0,
                                Second = hasSecond ? secondScore : 0,
                                Third = hasThird ? thirdScore : 0
                            };
                        }
                        else
                        {
                            row.SubjectTotals[sub.SubjectId] = null;
                        }
                    }

                    row.AnnualTotal = Math.Round(studentTotals.TryGetValue(studentId, out var total) ? total : 0, 2);
                    row.AnnualAverage = Math.Round(row.AnnualTotal / maxScore * 100.0, 2);
                    row.Position = ranks.TryGetValue(studentId, out var rank) ? rank : 0;
                    row.Grade = SD.Grade(row.AnnualAverage);
                    row.Remark = SD.Remark(row.AnnualAverage);

                    rows.Add(row);
                }

                StudentRows = rows;
                StudentCount = rows.Count;

                // 9. Footer statistics.
                SubjectSummaries = BuildSubjectSummaries(results);

                ExcelFileName = $"{BannerSession.Replace("/", "_")}_Annual_Master_Sheet";

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to generate the annual master sheet for session {Session}, class {Class}, subclass {SubClass}.",
                    CTSSClass.SessionYear, CTSSClass.Classes, CTSSClass.Subclass);
                TempData["error"] = "Sorry, the annual master sheet could not be generated at this time. Please try again later.";
                return Page();
            }
        }

        private IList<SubjectSummary> BuildSubjectSummaries(IEnumerable<ResultTable> results)
        {
            var summaries = new List<SubjectSummary>();
            var subjectIds = Subjects.Select(s => s.SubjectId).ToHashSet();

            foreach (var subjectId in subjectIds)
            {
                var subjectResults = results.Where(r => r.SubjectId == subjectId).ToList();
                if (subjectResults.Count == 0)
                {
                    continue;
                }

                summaries.Add(new SubjectSummary
                {
                    SubjectId = subjectId,
                    SubjectName = Subjects.First(s => s.SubjectId == subjectId).SubjectName,
                    FirstAverage = TermlyAverage(subjectResults, SD.First),
                    SecondAverage = TermlyAverage(subjectResults, SD.Second),
                    ThirdAverage = TermlyAverage(subjectResults, SD.Third),
                    AnnualAverage = AnnualSubjectAverage(subjectResults),
                    Highest = HighestLowest(subjectResults).Highest,
                    Lowest = HighestLowest(subjectResults).Lowest
                });
            }

            return summaries;
        }

        /// <summary>Average total of one subject in one term across all students who sat it.</summary>
        private static double TermlyAverage(IEnumerable<ResultTable> subjectResults, string term)
        {
            var rows = subjectResults.Where(r => r.Termregistration.Term == term).ToList();
            return SafeAverage(rows.Select(r => r.Total ?? 0));
        }

        /// <summary>
        /// Annual subject average: first averages each student's term totals across the terms
        /// they actually sat, then averages those per-student averages for the class.
        /// </summary>
        private static double AnnualSubjectAverage(IEnumerable<ResultTable> subjectResults)
        {
            var perStudentAverages = subjectResults
                .GroupBy(r => r.Termregistration.StudentId)
                .Select(g => g.Sum(r => r.Total ?? 0) / (double)g.Select(r => r.Termregistration.Term).Distinct().Count());
            return SafeAverage(perStudentAverages);
        }

        private static (double Highest, double Lowest) HighestLowest(IEnumerable<ResultTable> subjectResults)
        {
            var studentTotals = subjectResults
                .GroupBy(r => r.Termregistration.StudentId)
                .Select(g => g.Sum(r => r.Total ?? 0))
                .ToList();

            return studentTotals.Count == 0
                ? (0, 0)
                : (studentTotals.Max(), studentTotals.Min());
        }

        private static double SafeAverage(IEnumerable<double> values)
        {
            var list = values as ICollection<double> ?? values.ToList();
            return list.Count == 0 ? 0 : Math.Round(list.Average(), 2);
        }

        private static string BuildFullName(StudentsData student)
        {
            if (student == null)
            {
                return "—";
            }
            return string.Join(" ", new[]
                {
                    student.SurName,
                    student.OtherName,
                    student.FirstName
                }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
        }

        private void LoadFilterOptions()
        {
            ViewSelectModel = new ViewSelectModel
            {
                Class = _db.SchoolClasses.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SubClass = _db.SubClasses.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SessionYearz = _db.SessionYears.OrderByDescending(k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }

        public class SubjectInfo
        {
            public int SubjectId { get; set; }
            public string SubjectName { get; set; } = string.Empty;
        }

        /// <summary>Term totals for one student in one subject.</summary>
        public class SubjectTotals
        {
            public double First { get; set; }
            public double Second { get; set; }
            public double Third { get; set; }
            public double AnnualTotal => First + Second + Third;
        }

        public class StudentRow
        {
            public int SerialNo { get; set; }
            public int StudentId { get; set; }
            public string AdmissionNo { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string Gender { get; set; } = string.Empty;

            /// <summary>Key = SubjectId; null entry = student has no score recorded for that subject.</summary>
            public Dictionary<int, SubjectTotals?> SubjectTotals { get; set; } = new();

            public double AnnualTotal { get; set; }
            public double AnnualAverage { get; set; }
            public int Position { get; set; }
            public string Grade { get; set; } = "—";
            public string Remark { get; set; } = "—";
        }

        public class SubjectSummary
        {
            public int SubjectId { get; set; }
            public string SubjectName { get; set; } = string.Empty;
            public double FirstAverage { get; set; }
            public double SecondAverage { get; set; }
            public double ThirdAverage { get; set; }
            public double AnnualAverage { get; set; }
            public double Highest { get; set; }
            public double Lowest { get; set; }
        }
    }
}
