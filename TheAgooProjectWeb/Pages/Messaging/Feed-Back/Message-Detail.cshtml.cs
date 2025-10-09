using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel.Models;

namespace TheAgooProjectWeb.Pages.Messaging.Feed_Back
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer},{SD.Role_Accountant}")]
    public class Message_DetailModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public FeedBack feedBack { get; set; }
        public Message_DetailModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet(int id)
        {
            feedBack = dbContext.FeedBacks.FirstOrDefault(j => j.Id == id);
            if (feedBack == null)
            {
                TempData["error"] = "Unidentified feed back message";
                return RedirectToPage("Index");
            }
            feedBack.IsRead = true;
            dbContext.SaveChanges();
            return Page();
        }
    }
}
