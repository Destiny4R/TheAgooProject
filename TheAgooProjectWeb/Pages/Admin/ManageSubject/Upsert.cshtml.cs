using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Admin.ManageSubject
{
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public Subjects subjects { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            if (id > 0)
            {
                subjects = dbContext.Subjects.FirstOrDefault(g => g.Id == id);
            }
            else
            {
                subjects = new Subjects();
            }
        }
        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(subjects.Name))
            {
                var check = dbContext.Subjects.FirstOrDefault(m => m.Name == subjects.Name);
                if (check != null)
                {
                    TempData["error"] = "Subject already exist!";
                    return Page();
                }
                if (subjects.Id > 0)
                {
                    var classNSub = dbContext.Subjects.Find(subjects.Id);
                    classNSub.Name = subjects.Name;
                    dbContext.Update(classNSub);
                    dbContext.SaveChanges();
                    TempData["success"] = "Subject Successfully Updated!";
                    return RedirectToPage("Index");
                }
                else
                {
                    subjects = new()
                    {
                        Name = subjects.Name
                    };
                    dbContext.Add(subjects);
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
            return Page();
        }
    }
}
