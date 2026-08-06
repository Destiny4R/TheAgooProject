using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.ultilities
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer},{SD.Role_Accountant},{SD.IsStudent}")]
    public class Pta_Payment_InvoiceModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public PTATable invoice {  get; set; }
        public string signature { get; set; }
        public Pta_Payment_InvoiceModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            if (id > 0)
            {
                invoice = dbContext.PTATables.Include(m=>m.PTAFee).Include(n => n.Termregistration.StudentsData).Include(n => n.ApplicationUser).Include(n => n.Termregistration.SessionYear).Include(n => n.Termregistration.SubClasses).Include(n => n.Termregistration.Schoolclasses).FirstOrDefault(k => k.Id == id);
                if (invoice == null)
                {
                    TempData["error"] = "Receipt information not found";
                    return RedirectToPage("/Admin/Fees-Manager/PTA-Fees/Index");
                }
                var user = await userManager.GetUserAsync(User);
                if (user != null && User.IsInRole(SD.IsStudent) && invoice.Termregistration.StudentsData.ApplicationUserId != user.Id)
                {
                    TempData["error"] = "Receipt information not found";
                    return RedirectToPage("/student/PTA-Fees-Payment");
                }
                var admin = await userManager.GetUsersInRoleAsync(SD.Role_Admin);
                if (admin.Any())
                {
                    var newAdmin = admin.FirstOrDefault();
                    var settings = dbContext.Settings.FirstOrDefault(k => k.ApplicationUserId == newAdmin.Id);
                    if (settings != null)
                    {
                        signature = settings.CashierSignature;
                    } 
                }
                return Page();
            }
            TempData["error"] = "Receipt information not found";
            return RedirectToPage("/Admin/Fees-Manager/PTA-Fees/Index");
        }
    }
}
