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
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer}")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<IndexModel> logger;

        public ViewSelectModel ViewSelectModel { get; set; }

        public Subjects subject { get; set; }
        public IndexModel(ApplicationDbContext dbContext, ILogger<IndexModel> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            getViewSelect();
        }
        public void OnGet()
        {

        }
        public IActionResult OnPostManageSubject(List<int> subjects)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid request.";
                return Page();
            }

            if (subjects == null || !subjects.Any())
            {
                TempData["error"] = "Please select at least one subject.";
                return Page();
            }

            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                // Step 1: Reset all subjects
                dbContext.Subjects.ExecuteUpdate(s => s.SetProperty(x => x.Register_State, false));

                // Step 2: Retrieve only the selected subjects
                var selectedSubjects = dbContext.Subjects.Where(x => subjects.Contains(x.Id)).ToList();

                foreach (var subject in selectedSubjects)
                {
                    subject.Register_State = true;
                }

                dbContext.SaveChanges();

                transaction.Commit();

                TempData["success"] = $"{selectedSubjects.Count} subject(s) successfully set for registration.";
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                logger.LogError(ex, "ERROR IN SUBJECT REGISTRATION");
                TempData["error"] = "Error processing your request.";
            }

            return Page();
        }


        public IActionResult OnPostAddAndUpdate(Subjects subject)
        {
            if (!string.IsNullOrEmpty(subject.Name))
            {
                var check = dbContext.Subjects.FirstOrDefault(m => m.Name == subject.Name);
                if (check != null)
                {
                    TempData["error"] = "Subject already exist!";
                    return RedirectToPage();
                }
                if (subject.Id > 0)
                {
                    var classNSub = dbContext.Subjects.Find(subject.Id);
                    classNSub.Name = subject.Name;
                    dbContext.Update(classNSub);
                    dbContext.SaveChanges();
                    TempData["success"] = "Subject Successfully Updated!";
                    return RedirectToPage("Index");
                }
                else
                {
                    subject = new()
                    {
                        Name = subject.Name
                    };
                    dbContext.Add(subject);
                    int result = 0;
                    try
                    {
                        result = dbContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        TempData["success"] = "Subject Already Added!";
                    }
                    if (result > 0)
                    {
                        TempData["success"] = "Subject Successfully Added!";
                        return RedirectToPage(nameof(Index));
                    }
                }
            }
            return RedirectToPage();
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
