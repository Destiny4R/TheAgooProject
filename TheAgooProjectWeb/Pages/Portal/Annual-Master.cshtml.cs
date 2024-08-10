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
    [Authorize]
    public class Annual_MasterModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public IEnumerable<ResultTable> Results { get; set; }
        public IList<Subectx> _subjects{get; set;}
        public IList<ResultSummary> _resultSummary { get; set; }
        public IList<ResultSummary> _resultSummary2 { get; set; }
        public IList<StudentScore> _studentscore { get; set; }
        public Annual_MasterModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet()
        {
            getViewSelect();
            _subjects = new List<Subectx>();
            return Page();
        }
        public IActionResult OnPost()
        {
            getViewSelect();
            if (CTSSClass.Classes>0 && CTSSClass.SessionYear>0 && CTSSClass.Subclass>0)
            {
                Results = dbContext.ResultTable.Include(k => k.Subjects).Include(k => k.Termregistration.StudentsData).Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.SubClasses).Include(k => k.Termregistration.Schoolclasses).Where(s => s.Termregistration.SessionYearId == CTSSClass.SessionYear && s.Termregistration.ClassesInSchoolId == CTSSClass.Classes && s.Termregistration.SubClassId == CTSSClass.Subclass).ToList();

                if (Results.Count()<1)
                {
                    TempData["error"] = "Result might not be ready for the select information or wrong combinations, check and try again";
                    _subjects = new List<Subectx>();
                    return Page();
                }
                _subjects = new List<Subectx>();
                foreach (var item in Results.GroupBy(j=>j.SubjectId))
                {
                    var subject = new Subectx
                    {
                        Subid = item.FirstOrDefault().SubjectId,
                        SubjectName = item.FirstOrDefault().Subjects.Name,
                        First = "First",
                        Second = "Second",
                        Third = "Third"
                    };
                    _subjects.Add(subject);
                }
                //GET ANNUAL POSTION BY CALCULATING ALL THE STUDENTS TOTAL SCORES AND RANKING
                _studentscore = new List<StudentScore>();
                foreach (var item in Results.GroupBy(j => j.Termregistration.StudentId))
                {
                    var student = new StudentScore
                    {
                        StudId = item.FirstOrDefault().Termregistration.StudentId
                    };
                    int allsubject = item.GroupBy(j => j.SubjectId).Count();
                    
                    foreach (var item1 in item)
                    {
						student.TotalScore += (double)(item1?.Total ?? 0);
					}
                    student.Average = Math.Round((student.TotalScore/item.Count()), 2);
                    _studentscore.Add(student);
                }
                _studentscore = _studentscore.Rank(p => p.Average, (p, rank) => new StudentScore {Average = p.Average, TotalScore = p.TotalScore, StudId = p.StudId, Rank = rank }).ToList();
                //GET EACH SUBJECT TERMLY SCORES
                _resultSummary = new List<ResultSummary>();
                foreach (var item in Results.GroupBy(j => j.Termregistration.Term))
                {
                    foreach (var item1 in item.GroupBy(k=>k.SubjectId))
                    {
                        var result = new ResultSummary();
                        foreach (var item2 in item1)
                        {
							result.TotalScore += (double)(item2?.Total ?? 0);
                        }
                        result.Term = item1.FirstOrDefault().Termregistration.Term;
                        result.SubId = item1.FirstOrDefault().SubjectId;
                        _resultSummary.Add(result);
                    }
                }

                //ANNUAL STUDENT HIHEST & LOWEST
                _resultSummary2 = new List<ResultSummary>();
                foreach (var item in Results.GroupBy(j => j.SubjectId))
                {
                    foreach (var item1 in item.GroupBy(k => k.Termregistration.StudentId))
                    {
                        var result = new ResultSummary();
                        foreach (var item2 in item1)
                        {
							result.TotalScore += (double)(item2?.Total ?? 0);
							//result.TotalScore += (double)item2.Total;
                        }
                        result.Term = item1.FirstOrDefault().Termregistration.Term;
                        result.SubId = item1.FirstOrDefault().SubjectId;
                        _resultSummary2.Add(result);
                    }
                }
            }
            return Page();
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                Class = dbContext.SchoolClasses.OrderByDescending(n=>n.Id).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SubClass = dbContext.SubClasses.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SessionYearz = dbContext.SessionYears.OrderBy(n => n.Id).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
        public class Subectx
        {
            public int Subid { get; set; }
            public string SubjectName { get; set; }
            public string First { get; set; }
            public string Second { get; set; }
            public string Third { get; set; }
        }
        public class StudentScore
        {
            public int StudId { get; set; }
            public double Average { get; set; }
            public double TotalScore { get; set; }
            public int Rank { get; set; }
        }
        public class ResultSummary
        {
            public int SubId { get; set; }
            public double TotalScore { get; set; }
            public string Term { get; set; }
        }
        public class HighLow
        {
            public int StudId { get; set; }
            public double TotalScore { get; set; }
        }
        public class D_Count
        {
            public int StudId { get; set; }
            public int Terms { get; set; }
        }
    }
}
