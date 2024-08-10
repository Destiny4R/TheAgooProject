using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Compute_Result
{
    [Authorize]
    public class Edit_ResultModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public ResultHolderVM result { get; set; }
        public Edit_ResultModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet(int id)
        {
            getViewSelect();
            if (id > 0)
            {
                //result = dbContext.ResultTable.Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.SubClasses).Include(k => k.Termregistration.Schoolclasses).Include(k => k.Termregistration.StudentsData).Include(k => k.Subjects).FirstOrDefault(i => i.Id == id);
                result = dbContext.ResultTable.Include(k=>k.Termregistration.StudentsData).Where(k=>k.Id==id).Select(k => new ResultHolderVM { 
                    Assignment = k.Assignment,
                    TermRegId = k.TermRegId,
                    Test = k.Test,
                    Project = k.Project,
                    ClassWork = k.ClassWork,
                    Examination = k.Examination,
                    SubjectsCode = k.SubjectId,
                    Grade = k.Grade,
                    Total = k.Total,
                    Remark = k.Remark,
                    Id = k.Id,
                    Status = k.Status,
                    Termregistration = k.Termregistration
                }).FirstOrDefault();
                if (result!=null)
                {
                    if (!result.Status)
                    {
                        TempData["error"] = "Result must be uploaded or added through excel file or via the adding page before editting";
                        return RedirectToPage("Detail-Result", (new { id = result.TermRegId }));
                    }
                    return Page();
                }
            }
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            getViewSelect();
            if (ModelState.IsValid)
            {
                if (result.Id > 0) {
                    var resultdata = dbContext.ResultTable.FirstOrDefault(n=>n.Id== result.Id);
                    if (resultdata != null) {
                        resultdata.Assignment = result.Assignment;
                        resultdata.Test = result.Test;
                        resultdata.Project = result.Project;
                        resultdata.Examination = result.Examination;
                        resultdata.ClassWork = result.ClassWork;
                        resultdata.Total = result.Total;
                        resultdata.SubjectId = result.SubjectsCode;
                        resultdata.Grade = SD.Grade((double)result.Total);
                        resultdata.Remark = SD.Remark((double)result.Total);
                        dbContext.Update(resultdata);
                        dbContext.SaveChanges();
                        TempData["success"] = "Result Update applied successfully";
                        return RedirectToPage("Detail-Result",(new {id = resultdata.TermRegId}));
                    }
                }
                
            }
            return Page();
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                Subjects = dbContext.Subjects.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
