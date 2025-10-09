using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectDataAccess;
using TheAgooProjectModel.ViewModels;
using TheAgooProjectModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TheAgooProjectWeb.Pages.Admin.Fees_Manager.other_pay.Special_Pay
{
    [Authorize(Roles = SD.Role_Admin)]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public SpecialPayTableVM specialPay { get; set; }
        public int hasValue = 0;
        public IEnumerable<SelectListItem>? OtherPay { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            getViewSelect();
            if (id > 0)
            {
                specialPay = dbContext.SpecialPaySetUps.Where(k => k.Id == id).Select(k => new SpecialPayTableVM()
                {
                    Id = k.Id,
                    Amount = k.Amount,
                    SessionYearId = k.SessionYearId,
                    ClassesInSchoolId = k.ClassesInSchoolId,
                    Term = k.Term,
                    OtherPayId = k.OtherPayId
                }).FirstOrDefault();

            }
            else
            {
                specialPay = new SpecialPayTableVM();
            }

        }
        public IActionResult OnPost()
        {
            getViewSelect();
            if (ModelState.IsValid)
            {
                if (specialPay.Id > 0)
                {
                    var payments = dbContext.AdditionalPayments.Where(k => k.SpecialpayId == specialPay.Id).ToList();
                    if (payments.Count > 0)
                    {
                        TempData["error"] = "Fees have been recorded under this amount. Editing this is not appropriate. Please remove all fees recorded under it before proceeding.!";
                        return Page();
                    }
                    try
                    {
                        var checkifaddedbefore = dbContext.SpecialPaySetUps.FirstOrDefault(k => k.Id == specialPay.Id);
                        if (checkifaddedbefore != null)
                        {
                            checkifaddedbefore.Amount = specialPay.Amount;
                            checkifaddedbefore.SessionYearId = specialPay.SessionYearId;
                            checkifaddedbefore.ClassesInSchoolId = specialPay.ClassesInSchoolId;
                            checkifaddedbefore.Term = specialPay.Term;
                            checkifaddedbefore.OtherPayId = specialPay.OtherPayId;
                            dbContext.Update(checkifaddedbefore);
                            dbContext.SaveChanges();
                            TempData["success"] = "Special Payment Fees Successfully Updated";
                            return RedirectToPage("Index");
                        }
                    }catch(Exception ex)
                    {
                        TempData["error"] = "We could process this at the moment try again later! ";
                        return Page();
                    }
                }
                else
                {
                    try
                    {
                        var checkifaddedbefore = dbContext.SpecialPaySetUps.FirstOrDefault(k => k.SessionYearId == specialPay.SessionYearId && k.Term == specialPay.Term && k.ClassesInSchoolId == specialPay.ClassesInSchoolId && k.OtherPayId==specialPay.OtherPayId);
                        if (checkifaddedbefore != null)
                        {
                            TempData["error"] = "Special payment Fee Already added for the provided class, check and try again!";
                            return Page();
                        }
                        var pay = new SpecialPaySetUp
                        {
                            Amount = specialPay.Amount,
                            SessionYearId = specialPay.SessionYearId,
                            ClassesInSchoolId = specialPay.ClassesInSchoolId,
                            Term = specialPay.Term,
                            OtherPayId = specialPay.OtherPayId
                        };
                        dbContext.Add(pay);
                        int result = dbContext.SaveChanges();
                        if (result > 0)
                        {
                            TempData["success"] = "Special Payment Fees Successfully Added";
                            return RedirectToPage("Index");
                        }
                    }
                    catch(Exception ex)
                    {
                        TempData["error"] = "We could process this at the moment try again later!";
                        return Page();
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
                SessionYearz = dbContext.SessionYears.OrderByDescending(k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
            OtherPay = dbContext.OtherPayTables.OrderByDescending(k => k.Name).Select(k => new SelectListItem
            {
                Text = k.Name,
                Value = k.Id.ToString()
            });
        }
    }
}
