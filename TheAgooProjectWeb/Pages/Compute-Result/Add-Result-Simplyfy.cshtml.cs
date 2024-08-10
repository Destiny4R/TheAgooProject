using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel.ViewModels;
using TheAgooProjectModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using static TheAgooProjectWeb.Pages.Compute_Result.IndexModel;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using TheAgooProjectDataAccess;
using Microsoft.AspNetCore.Authorization;

namespace TheAgooProjectWeb.Pages.Compute_Result
{
    [Authorize]
    public class Add_Result_SimplyfyModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public SettingNames settingNames;
        public int hasSomething = 0;
        [BindProperty]
        public IList<ResultTable> results {  get; set; }
        public Add_Result_SimplyfyModel(ApplicationDbContext dbContext)
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
                return Page();
            }
            settingNames = new();
            TempData["error"] = "Please go to settings and select the class you will be working on before proceeding!";
            return Page();
        }

        public IActionResult OnPostSearch(string regnumber)
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

                if (!string.IsNullOrEmpty(regnumber))
                {
                    results = dbContext.ResultTable.Include(c => c.Termregistration).Include(c => c.Subjects).Include(k => k.Termregistration.StudentsData).Include(c => c.Termregistration.Schoolclasses).Include(c => c.Termregistration.SubClasses).Where(w => w.Termregistration.StudentsData.StudentRegNo == regnumber && w.Termregistration.Term == settings.Term && w.Termregistration.SessionYearId==settings.sessionYear && w.Termregistration.ClassesInSchoolId==settings.Classes && w.Termregistration.SubClassId==settings.subclass).ToList();
                    if (results.Count() == 0)
                    {
                        TempData["error"] = "Student not found or yet to be register for the term";
                        return Page();
                    }
                    TempData["success"] = "Student found";
                    hasSomething = 2;
                    return Page();
                }
            }
            TempData["error"] = "Provide student registration number";
            return Page();
        }
        public IActionResult OnPostResult( int termregId)
        {
            int update = 0, notupdate = 0;
            foreach (var item in results)
            {
                var data = dbContext.ResultTable.FirstOrDefault(k => k.SubjectId == item.SubjectId && k.TermRegId == termregId);
                if (data != null)
                {
                    data.Assignment = item.Assignment;
                    data.Test = item.Test;
                    data.Project = item.Project;
                    data.ClassWork = item.ClassWork;
                    data.Examination = item.Examination;
                    data.Total = item.Total;
                    data.Grade = SD.Grade((double)item.Total);
                    data.Remark = SD.Remark((double)item.Total);
                    data.Status = true;
                    dbContext.Update(data);
                    update++;
                }
                else { notupdate++; }
            }
            string msg = "";
            if (notupdate > 0)
            {
                msg = $", but {notupdate} result(s) not updated, this may be due subject(s) not registered for this student";
            }
            if (update > 0)
            {
                dbContext.SaveChanges();
                TempData["success"]=  $" results successfully updated!{msg}";
                OnGet();
                return Page();
            }
            return Page();
        }
    }
}
