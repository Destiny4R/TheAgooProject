using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.PTA_Fees_Setup
{
    [Authorize(Roles = SD.Role_Admin)]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public PTAFees PtaFees { get; set; }
        public int hasValue = 0;
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet( int id)
        {
            getViewSelect();
            if (id > 0)
            {
                PtaFees = dbContext.PTAfees.FirstOrDefault(k => k.Id == id);

            }
            else
            {
                PtaFees = new PTAFees();
            }
        }
        public IActionResult OnPost()
        {
            getViewSelect();
            if (ModelState.IsValid)
            {
                if (PtaFees.Id > 0)
                {
                    var payments = dbContext.PTATables.Where(k => k.PTAFeesId == PtaFees.Id).ToList();
                    if (payments.Count > 0)
                    {
                        TempData["error"] = "Fees have been recorded under this amount. This is not appropriate. Please remove all fees recorded under it before proceeding.!";
                        return Page();
                    }
                    var checkifaddedbefore = dbContext.PTAfees.FirstOrDefault(k => k.Id == PtaFees.Id);
                    if (checkifaddedbefore != null)
                    {
                        checkifaddedbefore.Fees = PtaFees.Fees;
                        checkifaddedbefore.SessionYearId = PtaFees.SessionYearId;
                        checkifaddedbefore.Term = PtaFees.Term;
                        dbContext.Update(checkifaddedbefore);
                        dbContext.SaveChanges();
                        TempData["success"] = "PTA Termly Fees Successfully Updated";
                        return RedirectToPage("Index");
                    }

                }
                else
                {
                    var checkifaddedbefore = dbContext.PTAfees.FirstOrDefault(k => k.SessionYearId == PtaFees.SessionYearId && k.Term == PtaFees.Term);
                    if (checkifaddedbefore != null)
                    {
                        TempData["error"] = "PTA Termly Fees Already added for the provided Term and Sesion/Year, check and try again!";
                        return Page();
                    }
                    var ClaimsId = (ClaimsIdentity)User.Identity;
                    var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
                    var termly = new PTAFees
                    {
                        Fees = PtaFees.Fees,
                        SessionYearId = PtaFees.SessionYearId,
                        Term = PtaFees.Term,
                        ApplicationUserId = claim.Value
                    };
                    dbContext.Add(termly);
                    int result = dbContext.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "PTA Termly Fees Successfully Added";
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
                SessionYearz = dbContext.SessionYears.OrderByDescending (k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
