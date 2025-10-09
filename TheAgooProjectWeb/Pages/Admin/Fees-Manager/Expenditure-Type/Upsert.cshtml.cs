using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.Expenditure_Type
{
    [Authorize(Roles =$"{SD.Role_Admin},{SD.Role_Accountant}")]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public ExpenditureTypes expenditure {  get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            if (id > 0)
            {
                expenditure = dbContext.ExpenditureType.FirstOrDefault(k => k.Id == id);
            }
        }
        public IActionResult OnPost() {

            if (!string.IsNullOrEmpty(expenditure.Name))
            {
                if (expenditure.Id > 0)
                {
                    var expen = dbContext.ExpenditureType.FirstOrDefault(k => k.Id == expenditure.Id);
                    if (expen != null)
                    {
                        expen.Name = expenditure.Name;
                        dbContext.Update(expen);
                        int result = dbContext.SaveChanges();
                        if (result > 0)
                        {
                            TempData["success"] = "Expenditure title successfully updated";
                            return RedirectToPage("Index");
                        }
                    }
                }
                else
                {
                    var expendi = new ExpenditureTypes()
                    {
                        Name = expenditure.Name,
                    };
                    dbContext.Add(expendi);
                    int result = dbContext.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "Expenditure title successfully added";
                        return RedirectToPage("Index");
                    }
                }
            }
            TempData["error"] = "Error processing your request check and try again";
            return Page();
        }
    }
}
