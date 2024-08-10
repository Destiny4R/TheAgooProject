using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using static TheAgooProjectWeb.Pages.Compute_Result.IndexModel;

namespace TheAgooProjectWeb.Pages.Compute_Result.General_Class_Info
{
    public class AttendanceModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public SettingNames settingNames;
        public int ClassAttendance = 0;
        public int hasSomething = 0;
        [BindProperty]
        public IList<RemarkPosition> remarks { get; set; }
        public AttendanceModel(ApplicationDbContext dbContext)
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
                remarks = dbContext.RemarkPositions.Include(k=>k.Termregistration.StudentsData).Include(k => k.Termregistration.Schoolclasses).Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.SubClasses).Include(k => k.Termregistration).Where(n=>n.Termregistration.Term==settings.Term && n.Termregistration.SessionYearId==settings.sessionYear && n.Termregistration.ClassesInSchoolId==settings.Classes && n.Termregistration.SubClassId==settings.subclass).ToList();
                var classAttend = dbContext.GeneralClassTables.FirstOrDefault(m => m.SessionYearId == settings.sessionYear && m.Term == settings.Term && m.ClassId == settings.Classes && m.SubClassId == settings.subclass);
                if (classAttend!=null)
                {
                    ClassAttendance = (int)classAttend.TotalAttendance;
                }
                if(remarks.Count() > 0)
                {
                    hasSomething = 3;
                }
                return Page();
            }
            settingNames = new();
            TempData["error"] = "Please go to settings and select the class you will be working on before proceeding!";
            return Page();
        }
        public IActionResult OnPost(int studentattend)
        {
            if (remarks.Count() > 0)
            {
                var remark = new List<RemarkPosition>();
                foreach (var item in remarks)
                {
                    var newremark = dbContext.RemarkPositions.FirstOrDefault(j => j.TermRegId == item.TermRegId);
                    if (newremark != null)
                    {
                        newremark.Student_Attendance = item.Student_Attendance;
                        newremark.Absent = (studentattend - (int)item.Student_Attendance).ToString();
                        remark.Add(newremark);
                    }
                }
                if (remark.Count() > 0)
                {
                    dbContext.UpdateRange(remark);
                    dbContext.SaveChanges();
                    TempData["success"] = "Students Attendance successfully updated";
                    return RedirectToPage("/Compute-Result/Index");
                }
            }
            TempData["error"] = "No data send to the server for update, Student attendance update failed";
            return Page();
        }
    }
}
