using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.Fees_Setup
{
    [Authorize(Roles = SD.Role_Admin)]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public TermlyFeesVM termlyFees {  get; set; }
        public int hasValue = 0;
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            getViewSelect();
            if (id > 0)
            {
                termlyFees = dbContext.TermLyFeesPays.Where(k => k.Id == id).Select(k => new TermlyFeesVM()
                {
                    Id = k.Id,
                    Amount = k.Amount,
                    SessionYearId = k.SessionYearId,
                    ClassesInSchoolId = k.ClassesInSchoolId,
                    Term = k.Term
                }).FirstOrDefault();

            }
            else
            {
                termlyFees = new TermlyFeesVM();
            }
            
        }
        public IActionResult OnPost()
        {
            getViewSelect();
            if (ModelState.IsValid)
            {
                if (termlyFees.Id > 0)
                {
                    var payments = dbContext.Payments.Where(k => k.TermlyFeesId  == termlyFees.Id).ToList();
                    if (payments.Count > 0)
                    {
                        TempData["error"] = "Fees have been recorded under this amount. This is not appropriate. Please remove all fees recorded under it before proceeding.!";
                        return Page();
                    }
                    var checkifaddedbefore = dbContext.TermLyFeesPays.FirstOrDefault(k => k.Id == termlyFees.Id );
                    if (checkifaddedbefore != null)
                    {
                        checkifaddedbefore.Amount = termlyFees.Amount;
                        checkifaddedbefore.SessionYearId = termlyFees.SessionYearId;
                        checkifaddedbefore.ClassesInSchoolId = termlyFees.ClassesInSchoolId;
                        checkifaddedbefore.Term = termlyFees.Term;
                        dbContext.Update(checkifaddedbefore);
                        dbContext.SaveChanges();
                        TempData["success"] = "Termly Fees Successfully Updated";
                        return RedirectToPage("Index");
                    }

                }
                else
                {
                    var checkifaddedbefore = dbContext.TermLyFeesPays.FirstOrDefault(k => k.SessionYearId == termlyFees.SessionYearId && k.Term == termlyFees.Term && k.ClassesInSchoolId == termlyFees.ClassesInSchoolId);
                    if (checkifaddedbefore != null)
                    {
                        TempData["error"] = "Termly Fees Already added for the provided class, check and try again!";
                        return Page();
                    }
                    var termly = new TermlyFeesPay
                    {
                        Amount = termlyFees.Amount,
                        SessionYearId = termlyFees.SessionYearId,
                        ClassesInSchoolId = termlyFees.ClassesInSchoolId,
                        Term = termlyFees.Term
                    };
                    dbContext.Add(termly);
                    int result = dbContext.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "Termly Fees Successfully Added";
                        return RedirectToPage("Index");
                    }
                }
            }
            TempData["error"] = "Some information are missing, check and try again!";
            return Page();
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                Class = dbContext.SchoolClasses.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SubClass = dbContext.SubClasses.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SessionYearz = dbContext.SessionYears.OrderByDescending (k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
