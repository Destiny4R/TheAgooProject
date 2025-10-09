using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel.ViewModels;
using TheAgooProjectModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp;
using System.Security.Claims;
using TheAgooProjectDataAccess;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace TheAgooProjectWeb.Pages.Messaging.Send_SMS
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer},{SD.Role_Accountant}")]
    public class SMS_SendModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly ISmsSender smsSender;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public IEnumerable<ExpenditureTable> expenditures { get; set; }
        public int IsSearch = 0;
        public SMS_SendModel(ApplicationDbContext dbContext, IConfiguration configuration,
            ISmsSender smsSender)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.smsSender = smsSender;
        }
        public void OnGet()
        {
            getViewSelect();
            
        }
        public IActionResult OnPostSearch()
        {
            getViewSelect();
            if (!string.IsNullOrEmpty(CTSSClass.Term) && CTSSClass.SessionYear > 0)
            {
                var getstudent = dbContext.TermRegistrations.Include(k => k.StudentsData).Where(n => n.SessionYearId == CTSSClass.SessionYear && n.Term == CTSSClass.Term).ToList();
                if (getstudent.Any())
                {
                    smsData = new SmsData();
                    int keep = 0;
                    foreach (var item in getstudent)
                    {
                        var getnumber = dbContext.ParentStudents.Include(k => k.ParentTable).Include(k => k.Studentsdata).FirstOrDefault(o => o.Studentsdata.StudentRegNo == item.StudentsData.StudentRegNo);
                        if (getnumber != null)
                        {
                            keep++;
                            if (getstudent.Count() == keep)
                            {
                                smsData.PhoneNumber += _goodNumbers(getnumber.ParentTable.PhoneNumber);
                            }
                            else
                            {
                                smsData.PhoneNumber += _goodNumbers(getnumber.ParentTable.PhoneNumber) + ", ";
                            }
                        }
                    }
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostSend()
        {
            getViewSelect();
            if (smsData.Sender != null && smsData.Subject != null && smsData.Message != null && !string.IsNullOrEmpty(smsData.PhoneNumber))
            {
                var ClaimsId = (ClaimsIdentity)User.Identity;
                var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);

                string successM = "", errorM = "";
                int success = 0, error = 0;


                var intervals = configuration.GetSection("Credentials").Get<List<SmsCredentials>>();
                string username = intervals.FirstOrDefault().UserName;
                string password = intervals.FirstOrDefault().Password;

                var results = await smsSender.SendMessage(username, password, smsData.Message, smsData.Sender, smsData.PhoneNumber);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                Data data = JsonSerializer.Deserialize<Data>(results, options);

                if (!string.IsNullOrEmpty(data.Status))
                {
                    if (data.Status.ToLower() == "ok")
                    {
                        success++;
                        successM = data.Count + " successfully sent, total cost of " + data.Price;
                    }
                }
                if (!string.IsNullOrEmpty(data.Error))
                {
                    error++;
                    errorM = $" An error occured while sending the messages, check you account balance or {data.Error}";
                }


                if (success > 0 || error > 0)
                {
                    SmsTables smsTable = new()
                    {
                        ApplicationUserId = claim.Value,
                        Sender = smsData.Sender,
                        Subject = smsData.Subject,
                        Message = smsData.Message,
                        SmsCount = (int)data.Count,
                        Cost = (int)data.Count * (int)data.Price,
                        CreatedDate = DateTime.Now,
                    };
                    dbContext.Add(smsTable);
                    var result = dbContext.SaveChanges();

                    TempData["success"] = successM + errorM;
                    return RedirectToPage("Index");
                }
            }
            TempData["error"] = "Provide all fields in the SMS area";
            return Page();
        }
        string _goodNumbers(string number)
        {
            number = number.Remove(0, 1);
            string numbers2 = 234 + number;
            return numbers2;
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
                SessionYearz = dbContext.SessionYears.OrderByDescending (k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
        [BindProperty] 
        public SmsData smsData { get; set; }
        public class SmsData
        {
            public string Subject { get; set; }
            public string Message { get; set; }
            public string Sender { get; set; }
            public string PhoneNumber { get; set; }
        }
        public class Data
        {
            public string? Status { get; set; }
            public int? Count { get; set; }
            public double? Price { get; set; }
            public string? Error { get; set; }
            public string? Errno { get; set; }
        }
    }
}
