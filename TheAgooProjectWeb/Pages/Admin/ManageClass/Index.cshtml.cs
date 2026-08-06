using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Admin.ManageClass
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer}")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public SchoolClasses _class { get; set; }

        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void OnGet()
        {
            _class = new SchoolClasses();
        }

        public IActionResult OnPost(string customRadio)
        {
            if (string.IsNullOrWhiteSpace(_class?.Name))
            {
                TempData["error"] = "Class name is required";
                return RedirectToPage("Index");
            }

            try
            {
                bool isElective = customRadio == "Cando";

                if (_class.Id > 0)
                {
                    var classNSub = dbContext.SchoolClasses.FirstOrDefault(k => k.Id == _class.Id);
                    if (classNSub == null)
                    {
                        TempData["error"] = "Class record not found";
                        return RedirectToPage("Index");
                    }

                    classNSub.Name = _class.Name.Trim();
                    classNSub.Elective = isElective;
                    dbContext.Update(classNSub);
                    dbContext.SaveChanges();
                    TempData["success"] = "Class Successfully Updated!";
                    return RedirectToPage("Index");
                }

                var normalizedName = _class.Name.Trim();
                var check = dbContext.SchoolClasses.FirstOrDefault(m => m.Name == normalizedName);
                if (check != null)
                {
                    TempData["error"] = "Class already exist!";
                    return RedirectToPage("Index");
                }

                var schoolClass = new SchoolClasses
                {
                    Name = normalizedName,
                    Elective = isElective
                };

                dbContext.SchoolClasses.Add(schoolClass);
                int result = dbContext.SaveChanges();
                TempData[result > 0 ? "success" : "error"] = result > 0 ? "Class Successfully Added!" : "Error processing your request check and try again";
                return RedirectToPage("Index");
            }
            catch
            {
                TempData["error"] = "Error processing your request check and try again";
                return RedirectToPage("Index");
            }
        }
    }
}
