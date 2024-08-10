using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Compute_Result.General_Class_Info
{
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public GeneralClassTable general {  get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            getViewSelect();
            general = new GeneralClassTable();
            if (id > 0)
            {
                var gD = dbContext.GeneralClassTables.FirstOrDefault(j => j.Id == id);
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
                var getgeneral1 = new GeneralClassTable();
                getgeneral1.Term = general.Term;
                getgeneral1.SessionYearId = general.SessionYearId;
                getgeneral1.ClassId = general.ClassId;
                getgeneral1.SubClassId = general.SubClassId;
                getgeneral1.NextTermStart = general.NextTermStart;
                getgeneral1.Next_Term_Fees = general.Next_Term_Fees;
                getgeneral1.TermEnd = general.TermEnd;
                getgeneral1.ClassTeacher = general.ClassTeacher;
                getgeneral1.TotalAttendance = general.TotalAttendance;
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
                SessionYearz = dbContext.SessionYears.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
