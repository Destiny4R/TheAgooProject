using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Messaging.Feed_Back
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer},{SD.Role_Accountant}")]
    public class Compose_MessageModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IMailSender mailSender;

        [BindProperty]
        public MessageInput Message { get; set; }
        public Compose_MessageModel(ApplicationDbContext dbContext, IConfiguration configuration, IMailSender mailSender)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.mailSender = mailSender;
        }
        public void OnGet(int id)
        {
            var intervals = configuration.GetSection("EmailCredentials").Get<List<EmailCredentials>>().FirstOrDefault();
            Message = new MessageInput();
            Message.Sender = intervals.UserName;
            if (id > 0)
            {
                var feed = dbContext.FeedBacks.FirstOrDefault(j => j.Id == id);
                if (feed != null)
                {
                    Message.SendTo = feed.Email;
                    Message.Subject = "Re: " + feed.Subject;
                    Message.Message = $"<p style=\"color: rgb(100, 82, 122);\"> {feed.Name}</p><p style=\"color: rgb(100, 82, 122);\"> {feed.Subject} </p><p style=\"color: rgb(100, 82, 122);\"> {feed.Email} </p><p style=\"color: rgb(100, 82, 122);\"> {feed.Message}</p>";
                }
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var intervals = configuration.GetSection("EmailCredentials").Get<List<SmsCredentials>>().FirstOrDefault();
                var result = await mailSender.SendEmailAsync(Message.SendTo, Message.CarbonCopy, Message.Subject, Message.Message, intervals.UserName, intervals.Password);
                if (result)
                {
                    TempData["success"] = "Email successfully sent";
                    return RedirectToPage("Index");
                }
            }
            TempData["error"] = "An error occurred while sending email, try again";
            return RedirectToPage();
        }

        public class MessageInput
        {
            [Required]
            [EmailAddress]
            [StringLength(50)]
            public string Sender { get; set; }
            [Required]
            [EmailAddress]
            [StringLength(50)]
            public string SendTo { get; set; }
            [Required]
            public string Subject { get; set; }
            public string? CarbonCopy { get; set; }
            [Required]
            public string Message { get; set; }
        }
    }
}
