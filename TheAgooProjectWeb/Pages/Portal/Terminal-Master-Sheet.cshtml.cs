using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Portal
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer}")]
    public class Terminal_Master_SheetModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }

        public ViewSelectModel ViewSelectModel { get; set; }

        /// <summary>Selected class, used to decide the individual result sheet link (Senior vs Junior).</summary>
        public SchoolClasses SchoolClass { get; set; }

        /// <summary>Ordered list of subjects offered by this class/term.</summary>
        public IList<SubjectInfo> Subjects { get; set; } = new List<SubjectInfo>();

        /// <summary>
        /// The flat assessment-column names derived from the actual ResultTable fields
        /// (e.g. "Assignment", "Test", "Project", "C/Work", "Exam", "Total").
        /// </summary>
        public IList<string> AssessmentColumns { get; set; } = new List<string>();

        /// <summary>One row per student, pre-aggregated for the view.</summary>
        public IList<StudentRow> StudentRows { get; set; } = new List<StudentRow>();

        public string BannerSession { get; set; } = string.Empty;
        public string BannerTerm { get; set; } = string.Empty;
        public string BannerClass { get; set; } = string.Empty;
        public string BannerSubClass { get; set; } = string.Empty;

        public string ExcelFileName { get; set; } = "terminal_master_sheet";

        public Terminal_Master_SheetModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void OnGet()
        {
            getViewSelect();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            getViewSelect();

            if (CTSSClass.SessionYear <= 0 || CTSSClass.Classes <= 0 ||
                CTSSClass.Subclass <= 0 || string.IsNullOrWhiteSpace(CTSSClass.Term))
            {
                TempData["error"] = "Please select all filters (Session, Term, Class and Sub-Class).";
                return Page();
            }

            // 1. Load ALL results for the selection in a single query.
            var allResults = await dbContext.ResultTable
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
                    r.Termregistration.Term == CTSSClass.Term &&
                    r.Termregistration.ClassesInSchoolId == CTSSClass.Classes &&
                    r.Termregistration.SubClassId == CTSSClass.Subclass)
                .ToListAsync();

            if (!allResults.Any())
            {
                TempData["error"] = "No results found for the selected combination. Ensure results have been uploaded for this class and term.";
                return Page();
            }

            // 2. Banner labels.
            var firstResult = allResults.First();
            BannerSession = firstResult.Termregistration.SessionYear.Name;
            BannerTerm = firstResult.Termregistration.Term;
            BannerClass = firstResult.Termregistration.Schoolclasses.Name;
            BannerSubClass = firstResult.Termregistration.SubClasses.Name;
            ExcelFileName = $"{BannerSession.Replace("/", "_")}_{BannerTerm}_Terminal_Master_Sheet";

            // 3. Build ordered subject list (distinct, sorted by name).
            Subjects = allResults
                .GroupBy(r => r.SubjectId)
                .OrderBy(g => g.First().Subjects.Name)
                .Select(g => new SubjectInfo
                {
                    SubjectId = g.Key,
                    SubjectName = g.First().Subjects.Name
                })
                .ToList();

            // 4. Assessment column headers (from model fields, in marking order).
            AssessmentColumns = new List<string>
            {
                "Assignment",
                "Test",
                "Project",
                "C/Work",
                "Exam",
                "Total"
            };

            // 5. Load registrations that have no results yet, so ALL registered
            //    students appear (even with zero scores).
            var registeredStudents = await dbContext.TermRegistrations
                .AsNoTracking()
                .Include(t => t.StudentsData)
                .Where(t =>
                    t.SessionYearId == CTSSClass.SessionYear &&
                    t.Term == CTSSClass.Term &&
                    t.ClassesInSchoolId == CTSSClass.Classes &&
                    t.SubClassId == CTSSClass.Subclass)
                .ToListAsync();

            // 6. Compute per-student totals for ranking.
            var scoreLookup = allResults
                .GroupBy(r => r.TermRegId)
                .ToDictionary(g => g.Key, g => g.Sum(r => r.Total ?? 0));

            // 7. Rank using the existing SD.Rank extension (competition ranking).
            var rankList = scoreLookup
                .Select(kv => new { TermRegId = kv.Key, Total = kv.Value })
                .ToList()
                .Rank(p => p.Total, (p, rank) => new { p.TermRegId, p.Total, Rank = rank })
                .ToDictionary(x => x.TermRegId, x => x);

            // 8. Build result lookup: (TermRegId, SubjectId) → ResultTable row.
            var resultLookup = allResults.ToDictionary(r => (r.TermRegId, r.SubjectId));

            // 9. Build student rows (ordered by name).
            int sn = 0;
            var rows = new List<StudentRow>();
            foreach (var reg in registeredStudents.OrderBy(r =>
                         r.StudentsData.SurName + " " + r.StudentsData.OtherName + " " + r.StudentsData.FirstName))
            {
                sn++;
                var row = new StudentRow
                {
                    SerialNo = sn,
                    TermRegId = reg.Id,
                    StudentId = reg.StudentId,
                    AdmissionNo = reg.StudentsData.StudentRegNo ?? "—",
                    FullName = string.Join(" ",
                        new[] { reg.StudentsData.SurName, reg.StudentsData.OtherName, reg.StudentsData.FirstName }
                        .Where(s => !string.IsNullOrWhiteSpace(s))),
                    Gender = reg.StudentsData.Gender ?? string.Empty
                };

                double cumulativeTotal = 0;
                foreach (var sub in Subjects)
                {
                    if (resultLookup.TryGetValue((reg.Id, sub.SubjectId), out var rt))
                    {
                        var scores = new SubjectScores
                        {
                            Assignment = rt.Assignment ?? 0,
                            Test = rt.Test ?? 0,
                            Project = rt.Project ?? 0,
                            ClassWork = rt.ClassWork ?? 0,
                            Examination = rt.Examination ?? 0,
                            Total = rt.Total ?? 0
                        };
                        row.SubjectScores[sub.SubjectId] = scores;
                        cumulativeTotal += rt.Total ?? 0;
                    }
                    else
                    {
                        // Subject exists for class but student has no score yet.
                        row.SubjectScores[sub.SubjectId] = null;
                    }
                }

                row.CumulativeTotal = cumulativeTotal;
                row.SubjectCount = Subjects.Count;
                row.Average = Subjects.Count > 0 ? Math.Round(cumulativeTotal / Subjects.Count, 2) : 0;

                if (rankList.TryGetValue(reg.Id, out var ranked))
                {
                    row.Position = ranked.Rank;
                }
                else
                {
                    row.Position = 0;
                }

                row.Grade = SD.CalculateGrade(row.Average);
                row.Remark = SD.Remark(row.Average);

                rows.Add(row);
            }

            SchoolClass = dbContext.SchoolClasses.FirstOrDefault(k => k.Id == CTSSClass.Classes);
            StudentRows = rows;
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
                SubClass = dbContext.SubClasses.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SessionYearz = dbContext.SessionYears.OrderByDescending(k => k.Name).Select(k => new SelectListItem
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

        public class SubjectScores
        {
            public double Assignment { get; set; }
            public double Test { get; set; }
            public double Project { get; set; }
            public double ClassWork { get; set; }
            public double Examination { get; set; }
            public double Total { get; set; }
        }

        public class StudentRow
        {
            public int SerialNo { get; set; }
            public int TermRegId { get; set; }
            public int StudentId { get; set; }
            public string AdmissionNo { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string Gender { get; set; } = string.Empty;

            /// <summary>Key = SubjectId; null entry = no score recorded.</summary>
            public Dictionary<int, SubjectScores?> SubjectScores { get; set; } = new();

            public double CumulativeTotal { get; set; }
            public double Average { get; set; }
            public int SubjectCount { get; set; }
            public int Position { get; set; }
            public string Grade { get; set; } = "—";
            public string Remark { get; set; } = "—";
        }
    }
}
