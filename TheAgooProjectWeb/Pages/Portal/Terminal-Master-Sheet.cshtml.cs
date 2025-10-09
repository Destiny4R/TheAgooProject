using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
        public IEnumerable<ResultTable> Result {  get; set; }
        public IList<StudentScore> students { get; set; }
        public IList<Subjects> GetSubjects { get; set; }
        public string information { get; set; }
        public Terminal_Master_SheetModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            getViewSelect();
        }
        public IActionResult OnPost()
        {
            getViewSelect();
            if (ModelState.IsValid)
            {
                try
                {
                    Result = dbContext.ResultTable.Include(q => q.Termregistration.StudentsData).Include(q => q.Termregistration.SessionYear).Include(q => q.Termregistration.Schoolclasses).Include(q => q.Termregistration.SubClasses).Include(q => q.Subjects).Where(s => s.Termregistration.SessionYearId == CTSSClass.SessionYear && s.Termregistration.ClassesInSchoolId == CTSSClass.Classes && s.Termregistration.SubClassId == CTSSClass.Subclass && s.Termregistration.Term == CTSSClass.Term).ToList();
                    var getinfo = Result.FirstOrDefault();
                    information = getinfo.Termregistration.SessionYear.Name + "<br>"
                        + getinfo.Termregistration.Term + "<br>" +
                        getinfo.Termregistration.Schoolclasses.Name + "<br>" + getinfo.Termregistration.SubClasses.Name;
                    students = new List<StudentScore>();
                    foreach (var item in Result.GroupBy(q => q.TermRegId))
                    {
                        var student = new StudentScore();
                        student.StudId = item.FirstOrDefault().TermRegId;

                        foreach (var item1 in item)
                        {
                            student.TotalScore += (double)(item1?.Total ?? 0);
                        }
                        student.Average = student.TotalScore / item.Count();
                        students.Add(student);
                    }
                    students = students.Rank(p => p.Average, (p, rank) => new StudentScore { Average = p.Average, TotalScore = p.TotalScore, StudId = p.StudId, Rank = rank }).ToList();
                    GetSubjects = new List<Subjects>();
                    foreach (var item in Result.GroupBy(q => q.SubjectId))
                    {
                        var subj = new Subjects();
                        subj.SubjId = item.FirstOrDefault().SubjectId;
                        subj.Name = item.FirstOrDefault().Subjects.Name;
                        GetSubjects.Add(subj);
                    }
                }
                catch(Exception ex)
                {
                    TempData["error"] = "No results were found for the selected criteria. Please check and try again.";
                }
            }
            return Page();
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                Class = dbContext.SchoolClasses.OrderByDescending(n => n.Id).Select(k => new SelectListItem
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
        public class StudentScore
        {
            public int StudId { get; set; }
            public double Average { get; set; }
            public double TotalScore { get; set; }
            public int Rank { get; set; }
        }
        public class Subjects
        {
            public int SubjId { get; set; }
            public string Name { get; set; }
        }
    }
}
