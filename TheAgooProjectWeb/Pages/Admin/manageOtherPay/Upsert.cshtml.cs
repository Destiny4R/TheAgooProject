using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Admin.manageOtherPay
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Accountant}")]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public OtherPayTable otherPay { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int? id)
        {
            if(id > 0)
            {
                otherPay = dbContext.OtherPayTables.FirstOrDefault(g => g.Id == id);
            }
            else
            {
                otherPay = new OtherPayTable();
            }
        }
        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(otherPay.Name))
            {
                var check = dbContext.OtherPayTables.FirstOrDefault(m => m.Name == otherPay.Name);
                if (check != null)
                {
                    TempData["error"] = "Other pay type Name already exist!";
                    return Page();
                }
                if (otherPay.Id > 0)
                {
                    var other = dbContext.OtherPayTables.Find(otherPay.Id);
                    other.Name = otherPay.Name;
                    dbContext.Update(other);
                    dbContext.SaveChanges();
                    TempData["success"] = "Other pay type  Successfully Updated!";
                    return RedirectToPage("Index");
                }
                else
                {
                    otherPay = new()
                    {
                        Name = otherPay.Name
                    };
                    dbContext.Add(otherPay);
                    int result = 0;
                    try
                    {
                        result = dbContext.SaveChanges();
                        if (result > 0)
                        {
                            TempData["success"] = "Othe Pay Type Successfully Added!";
                            return RedirectToPage(nameof(Index));
                        }

                    }
                    catch (Exception ex)
                    {
                        TempData["success"] = "Other pay type Already Added!";
                    }
                }
            }
            return Page();
        }
    }
}
