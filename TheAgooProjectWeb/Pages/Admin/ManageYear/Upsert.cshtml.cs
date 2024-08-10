using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Admin.ManageYear
{
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public SessionYear sessionYear { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int? id)
        {
            if(id > 0)
            {
                sessionYear = dbContext.SessionYears.FirstOrDefault(g => g.Id == id);
            }
            else
            {
                sessionYear = new SessionYear();
            }
        }
        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(sessionYear.Name))
            {
                var check = dbContext.SessionYears.FirstOrDefault(m => m.Name == sessionYear.Name);
                if (check != null)
                {
                    TempData["error"] = "Session/Year already exist!";
                    return Page();
                }
                if (sessionYear.Id > 0)
                {
                    var classNSub = dbContext.SessionYears.Find(sessionYear.Id);
                    classNSub.Name = sessionYear.Name;
                    dbContext.Update(classNSub);
                    dbContext.SaveChanges();
                    TempData["success"] = "Session/Year Successfully Updated!";
                    return RedirectToPage("Index");
                }
                else
                {
                    sessionYear = new()
                    {
                        Name = sessionYear.Name
                    };
                    dbContext.SessionYears.Add(sessionYear);
                    int result = 0;
                    try
                    {
                        result = dbContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        TempData["success"] = "Session/Year Already Added!";
                    }
                    if (result > 0)
                    {
                        TempData["success"] = "Session/Year Successfully Added!";
                        return RedirectToPage(nameof(Index));
                    }
                }
            }
            return Page();
        }
    }
}
