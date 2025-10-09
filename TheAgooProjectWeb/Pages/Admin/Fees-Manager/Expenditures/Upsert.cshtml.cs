using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.Expenditures
{
    [Authorize(Roles =$"{SD.Role_Admin},{SD.Role_Accountant}")]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public ExpenseVM ExpenseVM { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet( int id)
        {
            getViewSelect();
            if (id > 0)
            {
                var expenses = dbContext.ExpenditureTables.Where(k => k.Id == id).Select(n => new ExpenseVM
                {
                    Id = n.Id,
                    Description = n.Description,
                    Amount = n.Amount,
                    TransactionDate = n.TransactionDate,
                    Term = n.Term,
                    ExpendId = n.ExpendId,
                    SessionYearId = n.SessionYearId
                }).FirstOrDefault();
                if (expenses != null)
                {
                    ExpenseVM = expenses;
                }
            }
        }
        public IActionResult OnPost()
        {
            getViewSelect();
            if (ModelState.IsValid)
            {
                if (ExpenseVM.Id > 0)
                {
                    var expendi = dbContext.ExpenditureTables.FirstOrDefault(m => m.Id == ExpenseVM.Id);
                    if (expendi != null)
                    {
                        expendi.Amount = ExpenseVM.Amount;
                        expendi.TransactionDate = ExpenseVM.TransactionDate;
                        expendi.SessionYearId = ExpenseVM.SessionYearId;
                        expendi.Term = ExpenseVM.Term;
                        expendi.Description = ExpenseVM.Description;
                        expendi.ExpendId = ExpenseVM.ExpendId;
                        dbContext.Update(expendi);
                        int result = dbContext.SaveChanges();
                        if (result > 0)
                        {
                            TempData["success"] = "Expenditure update successfully applied";
                            return RedirectToPage("Index");
                        }
                    }
                    TempData["error"] = "Expenditure update failed: Unidentified data";
                    return Page();
                }
                else
                {
                    var ClaimsId = (ClaimsIdentity)User.Identity;
                    var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
                    var expendi = new ExpenditureTable()
                    {
                        Amount = ExpenseVM.Amount,
                        TransactionDate = ExpenseVM.TransactionDate,
                        SessionYearId = ExpenseVM.SessionYearId,
                        Term = ExpenseVM.Term,
                        Description = ExpenseVM.Description,
                        ApplicationUserId = claim.Value,
                        ExpendId = ExpenseVM.ExpendId
                    };
                    dbContext.Add(expendi);
                    int result = dbContext.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "Expenditure successfully recorded";
                        return RedirectToPage("Index");
                    }
                }
            }
            TempData["error"] = "Error processing your request check and try again";
            return Page();
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                SessionYearz = dbContext.SessionYears.OrderByDescending (k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                Class = dbContext.ExpenditureType.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
