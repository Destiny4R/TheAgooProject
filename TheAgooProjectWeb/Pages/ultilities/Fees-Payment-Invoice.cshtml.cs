using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectDataAccess;
using TheAgooProjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TheAgooProjectWeb.Pages.ultilities
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer},{SD.Role_Accountant},{SD.IsStudent}")]
    public class Fees_Payment_InvoiceModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public Payment invoice { get; set; }
        public string signature { get; set; }
        public string Cashier { get; set; }
        public Fees_Payment_InvoiceModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            if (id > 0)
            {
                invoice = dbContext.Payments.Include(n=>n.TermlyFees).Include(n => n.Termregistrations.StudentsData).Include(n => n.Termregistrations.SessionYear).Include(n => n.Termregistrations.SubClasses).Include(n => n.Termregistrations.Schoolclasses).FirstOrDefault(k => k.Id == id);
                if (invoice == null)
                {
                    TempData["error"] = "Receipt information not found";
                    return RedirectToPage("/Admin/Fees-Manager/Fees-Payment/Index");
                }
                var user = await userManager.GetUserAsync(User);
                if (user != null && User.IsInRole(SD.IsStudent) && invoice.Termregistrations.StudentsData.ApplicationUserId != user.Id)
                {
                    TempData["error"] = "Receipt information not found";
                    return RedirectToPage("/student/Payment-Records");
                }
                var admin = await userManager.GetUsersInRoleAsync(SD.Role_Admin);
                if (admin.Any())
                {
                    var newAdmin = admin.FirstOrDefault();
                    var settings = dbContext.Settings.FirstOrDefault(k => k.ApplicationUserId == newAdmin.Id);
                    if (settings!=null)
                    {
                        signature = settings.CashierSignature;
                        var casheir = await userManager.FindByIdAsync(invoice.Cashier);
                        if(casheir != null)
                        {
                            Cashier = casheir.Fullname;
                        }
                    }
                }
                return Page();
            }
            TempData["error"] = "Receipt information not found";
            return RedirectToPage("/Admin/Fees-Manager/PTA-Fees/Index");
        }
    }
}
