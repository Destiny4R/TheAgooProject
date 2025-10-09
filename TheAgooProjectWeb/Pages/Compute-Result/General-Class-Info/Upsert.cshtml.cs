using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectDataAccess.Migrations;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Compute_Result.General_Class_Info
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer}")]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public GeneralClassInfoVM general {  get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            getViewSelect();
            if (id > 0)
            {
                var gD = dbContext.GeneralClassTables.Where(j=> j.Id==id).Select(n=> new GeneralClassInfoVM
                {
                    Id = n.Id,
                    ClassId = n.ClassId,
                    SessionYearId = n.SessionYearId,
                    Term = n.Term,
                    SubClassId= n.SubClassId,
                    NextTermStart = n.NextTermStart,
                    Next_Term_Fees = n.Next_Term_Fees,
                    PrincipalName = n.PrincipalName,
                    ClassTeacher = n.ClassTeacher,
                    TermEnd = n.TermEnd,
                    TotalAttendance = n.TotalAttendance,
				}).FirstOrDefault();
                if (gD != null)
                {
                    general = gD;
                }
            }
        }
        public IActionResult OnPost()
        {
            getViewSelect();
            if (general.Id > 0)
            {
                var getgeneral = dbContext.GeneralClassTables.FirstOrDefault(k => k.Id == general.Id);
                if(getgeneral != null)
                {
                    getgeneral.Term = general.Term;
                    getgeneral.SessionYearId = general.SessionYearId;
                    getgeneral.ClassId = general.ClassId;
                    getgeneral.SubClassId = general.SubClassId;
                    getgeneral.NextTermStart = general.NextTermStart;
                    getgeneral.Next_Term_Fees = general.Next_Term_Fees;
                    getgeneral.TermEnd = general.TermEnd;
                    getgeneral.ClassTeacher = general.ClassTeacher;
                    getgeneral.TotalAttendance = general.TotalAttendance;
                    getgeneral.PrincipalName = general.PrincipalName;
                    dbContext.Update(getgeneral);
                    dbContext.SaveChanges();
                    TempData["success"] = "Class general information successfully updated";
                    return RedirectToPage("Index");
                }
            }
            else
            {
                var checkifexist = dbContext.GeneralClassTables.FirstOrDefault(k => k.Term == general.Term && k.SessionYearId == general.SessionYearId && k.ClassId == general.ClassId && k.SubClassId == general.SubClassId);
                if (checkifexist != null)
                {
                    TempData["error"] = "General class information for the selected information has alrady been added";
                    return Page();
                }
                var getgeneral1 = new GeneralClassTable
                {
                    Term = general.Term,
                    SessionYearId = general.SessionYearId,
                    ClassId = general.ClassId,
                    SubClassId = general.SubClassId,
                    NextTermStart = general.NextTermStart,
                    Next_Term_Fees = general.Next_Term_Fees,
                    TermEnd = general.TermEnd,
                    ClassTeacher = general.ClassTeacher,
                    TotalAttendance = general.TotalAttendance,
				    PrincipalName = general.PrincipalName,
                };
				dbContext.Add(getgeneral1);
                dbContext.SaveChanges();
                TempData["success"] = "Class general information successfully added";
                return RedirectToPage("Index");
            }
            TempData["error"] = "An error occured while processing your request, try again later";
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
                SessionYearz = dbContext.SessionYears.OrderByDescending (k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
