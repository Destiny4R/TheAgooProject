using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Admin.ManageSubject
{
        [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer}")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public ViewSelectModel ViewSelectModel { get; set; }
        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            getViewSelect();
        }
        public IActionResult OnPost(List<int> subjects)
        {
            getViewSelect();
            if (ModelState.IsValid)
            {
                if (subjects.Count > 0)
                {
                    var subject = dbContext.Subjects.ExecuteUpdate(s => s.SetProperty(k => k.Register_State, k => false));
                    if (subject > 0)
                    {
                        var subs = new List<Subjects>();
                        var subData = dbContext.Subjects.ToList();
                        foreach (var item in subjects)
                        {
                            var subx = subData.FirstOrDefault(j => j.Id == item);
                            if (subx != null)
                            {
                                subx.Register_State = true;
                                subs.Add(subx);
                            }
                        }
                        if (subs.Count > 0)
                        {
                            dbContext.UpdateRange(subs);
                            int result = dbContext.SaveChanges();
                            if (result > 0)
                            {
                                TempData["success"] = $"{result} Subjects successfully set for registration";
                                return Page();
                            }
                        }
                    }
                }
            }
            TempData["error"] = "Error in processing your request";
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
