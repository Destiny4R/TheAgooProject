using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using static TheAgooProjectWeb.Pages.Compute_Result.IndexModel;

namespace TheAgooProjectWeb.Pages.Compute_Result
{
    [Authorize]
    public class Subjects_PositionModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public SettingNames settingNames;
        public int DataTrue = 0;
        public IEnumerable<SelectListItem> Subjects { get; set; }
        public IEnumerable<ResultTable> ResultTables { get; set; }
        public Subjects_PositionModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet()
        {
            var ClaimFinder = (ClaimsIdentity)User.Identity;
            var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);

            var settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == claim.Value);
            if (settings != null)
            {
                settingNames = new()
                {
                    Sessionyear = dbContext.SessionYears.Find(settings.sessionYear).Name,
                    Term = settings.Term,
                    Classes = dbContext.SchoolClasses.Find(settings.Classes).Name,
                    Subclass = dbContext.SubClasses.Find(settings.subclass).Name,
                };
                var _GetResult2 = dbContext.ResultTable.Include(c => c.Termregistration.SessionYear).Include(c => c.Subjects).Include(k => k.Termregistration.StudentsData).Include(c => c.Termregistration.Schoolclasses).Include(c => c.Termregistration.SubClasses).Where(h => h.Termregistration.SessionYearId == settings.sessionYear && h.Termregistration.ClassesInSchoolId == settings.Classes && h.Termregistration.SubClassId == settings.subclass && h.Termregistration.Term == settings.Term).ToList();

                var subject = new List<SubjectData>();
                foreach (var item in _GetResult2.GroupBy(k=>k.SubjectId))
                {
                    var getSubject = new SubjectData()
                    {
                        Name = item.FirstOrDefault().Subjects.Name,
                        Id = item.FirstOrDefault().SubjectId
                    };
                    subject.Add(getSubject);
                }

                Subjects = subject.Select(k => new SelectListItem { Text = k.Name, Value = k.Id.ToString() });

                return Page();
            }
            settingNames = new();
            TempData["error"] = "Please go to settings and select the class you will be working on before proceeding!";
            return RedirectToPage("Index");
        }
        public IActionResult OnPostSearch( int subjectId) 
        {
            if (subjectId>0)
            {
                var ClaimFinder = (ClaimsIdentity)User.Identity;
                var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);

                var settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == claim.Value);
                if (settings != null)
                {
                    settingNames = new()
                    {
                        Sessionyear = dbContext.SessionYears.Find(settings.sessionYear).Name,
                        Term = settings.Term,
                        Classes = dbContext.SchoolClasses.Find(settings.Classes).Name,
                        Subclass = dbContext.SubClasses.Find(settings.subclass).Name,
                    };

                    var _GetResult2 = dbContext.ResultTable.Include(c => c.Termregistration.SessionYear).Include(c => c.Subjects).Include(k => k.Termregistration.StudentsData).Include(c => c.Termregistration.Schoolclasses).Include(c => c.Termregistration.SubClasses).Where(h => h.Termregistration.SessionYearId == settings.sessionYear && h.Termregistration.ClassesInSchoolId == settings.Classes && h.Termregistration.SubClassId == settings.subclass && h.Termregistration.Term == settings.Term).ToList();

                    var subject = new List<SubjectData>();
                    foreach (var item in _GetResult2.GroupBy(k => k.SubjectId))
                    {
                        var getSubject = new SubjectData()
                        {
                            Name = item.FirstOrDefault().Subjects.Name,
                            Id = item.FirstOrDefault().SubjectId
                        };
                        subject.Add(getSubject);
                    }
                    Subjects = subject.Select(k => new SelectListItem { Text = k.Name, Value = k.Id.ToString() });
                    ResultTables = _GetResult2.Where(k => k.SubjectId == subjectId).ToList();
                    if (ResultTables.Count() > 0)
                    {
                        DataTrue = 2;
                        return Page();
                    }
                }
            }
            TempData["error"] = "“Sorry, we couldn’t retrieve what you are looking for at the moment. Please try again later.”";
            return RedirectToPage();
        }

        public IActionResult OnPostPosition()
        {
            var ClaimFinder = (ClaimsIdentity)User.Identity;
            var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);

            var settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == claim.Value);
            if (settings != null)
            {
                var _GetResult2 = dbContext.ResultTable.Include(c => c.Termregistration.SessionYear).Include(c => c.Subjects).Include(k => k.Termregistration.StudentsData).Include(c => c.Termregistration.Schoolclasses).Include(c => c.Termregistration.SubClasses).Where(h => h.Termregistration.SessionYearId == settings.sessionYear && h.Termregistration.ClassesInSchoolId == settings.Classes && h.Termregistration.SubClassId == settings.subclass && h.Termregistration.Term == settings.Term).ToList();

                //COMPUTE POSITION IN SUBJECT
                if (_GetResult2.Count() > 0)
                {
                    var getDatasub = new List<SubjectRank>();
                    int succeed = 0;
                    foreach (var item in _GetResult2.GroupBy(m=>m.SubjectId).ToList())
                    {

                        foreach (var item1 in item)
                        {
                            var item2 = new SubjectRank
                            {
                                Id = item1.Id,
                                SubId = item1.SubjectId,
                                TotalScore = (double)item1.Total
                            };
                            getDatasub.Add(item2);
                        }

                    }
                    var findata = new List<ResultTable>();
                    foreach(var item in getDatasub.GroupBy(k=>k.SubId))
                    {
                        foreach (var item1 in RankSubjects(item.ToList()))
                        {
                            var getdata = _GetResult2.FirstOrDefault(k => k.Id == item1.Id);
                            if (getdata != null)
                            {
                                getdata.Position = item1.Rank;
                                getdata.Status = true;
                                findata.Add(getdata);
                            }
                        }
                    }
                    if (findata.Count() > 0) 
                    {
                        dbContext.UpdateRange(findata);
                        dbContext.SaveChanges();
                        TempData["success"] = "Subjects postions has been successfully computed and updated!";
                        return RedirectToPage();
                    }
                }
            }
            TempData["error"] = "An error occured while computing the result, please try again!";
            return RedirectToPage();
        }


        public class SubjectData
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
        public class SubjectRank
        {
            public double TotalScore { get; set; }
            public int Id { get; set; }
            public int SubId { get; set; }
            public int Rank { get; set; }
        }
        public  List<SubjectRank> RankSubjects(List<SubjectRank> subjects)
        {
            if (subjects == null || !subjects.Any())
            {
                return new List<SubjectRank>();
            }

            subjects = subjects.OrderByDescending(s => s.TotalScore).ToList();

            int currentRank = 1;
            int previousScore = int.MaxValue; // Initialize with a high value

            for (int i = 0; i < subjects.Count; i++)
            {
                int currentScore = (int)Math.Floor(subjects[i].TotalScore); // Extract integer part of score

                // Check for ties or decrease in score
                if (currentScore == previousScore)
                {
                    subjects[i].Rank = currentRank;
                }
                else
                {
                    currentRank = i + 1; // Skip ranks for ties
                    subjects[i].Rank = currentRank;
                }

                previousScore = currentScore;
            }

            return subjects;
        }

    }
}
