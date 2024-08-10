using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Admin.ManageSubClass
{
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public SubClasses subClass { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int? id)
        {
            if (id > 0)
            {
                subClass = dbContext.SubClasses.FirstOrDefault(g => g.Id == id);
            }
            else
            {
                subClass = new SubClasses();
            }
        }
        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(subClass.Name))
            {
                var check = dbContext.SubClasses.FirstOrDefault(m => m.Name == subClass.Name);
                if (check != null)
                {
                    TempData["error"] = "Sub-Class already exist!";
                    return Page();
                }
                if (subClass.Id > 0)
                {
                    var classNSub = dbContext.SubClasses.Find(subClass.Id);
                    classNSub.Name = subClass.Name;
                    dbContext.Update(classNSub);
                    dbContext.SaveChanges();
                    TempData["success"] = "Sub-Class Successfully Updated!";
                    return RedirectToPage("Index");
                }
                else
                {
                    subClass = new()
                    {
                        Name = subClass.Name
                    };
                    dbContext.Add(subClass);
                    int result = 0;
                    try
                    {
                        result = dbContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        TempData["success"] = "Sub-Class Already Added!";
                    }
                    if (result > 0)
                    {
                        TempData["success"] = "Sub-Class Successfully Added!";
                        return RedirectToPage(nameof(Index));
                    }
                }
            }
            return Page();
        }
    }
}
