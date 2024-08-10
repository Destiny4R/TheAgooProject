using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Admin.ManageClass
{
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public SchoolClasses _class { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            if (id > 0)
            {
                _class = dbContext.SchoolClasses.FirstOrDefault(g => g.Id == id);
            }
            else
            {
                _class = new SchoolClasses();
            }
        }
        public IActionResult OnPost(string customRadio)
        {
            if (!string.IsNullOrEmpty(_class.Name))
            {
                if (_class.Id > 0)
                {
                    var classNSub = dbContext.SchoolClasses.Find(_class.Id);
                    classNSub.Name = _class.Name;
                    if (customRadio== "Cando")
                    {
                        classNSub.Elective = true;
                    }
                    else
                    {
                        classNSub.Elective = false;
                    }
                    dbContext.Update(classNSub);
                    dbContext.SaveChanges();
                    TempData["success"] = "Class Successfully Updated!";
                    return RedirectToPage("Index");
                }
                else
                {
                    var check = dbContext.SchoolClasses.FirstOrDefault(m => m.Name == _class.Name);
                    if (check != null)
                    {
                        TempData["error"] = "Class already exist!";
                        return Page();
                    }
                    _class = new()
                    {
                        Name = _class.Name,
                        Elective = _class.Elective
                    };
                    dbContext.SchoolClasses.Add(_class);
                    int result = 0;
                    try
                    {
                        result = dbContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        TempData["success"] = "Class Already Added!";
                    }
                    if (result > 0)
                    {
                        TempData["success"] = "Class Successfully Added!";
                        return RedirectToPage(nameof(Index));
                    }
                }
            }
            return Page();
        }
    }
}
