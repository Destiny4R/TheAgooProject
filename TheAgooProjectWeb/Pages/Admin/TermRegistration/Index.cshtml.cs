using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;
using static TheAgooProjectWeb.Pages.Messaging.Send_SMS.SMS_SendModel;

namespace TheAgooProjectWeb.Pages.Admin.TermRegistration
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer}")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public bool CheckAll2 { get; set; }

        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public CTSSClass CTSSClass { get; set; }

        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            getViewSelect();
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost(List<IncomingData> datas)
        {
            if (datas.Any())
            {
                int yes = 0, no = 0;
                var regStudents = new List<TheAgooProjectModel.TermRegistration>();
                var regResult = new List<ResultTable>();
                foreach (var data in datas.Where(l=>l.ischeck))
                {
                    var student = dbContext.PaymentReports.FirstOrDefault(k => k.TermRegId == data.Id && data.ischeck);
                    if (student != null)
                    {
                        no++;
                    }
                    var studentpta = dbContext.PTAFeesReports.FirstOrDefault(k => k.TermRegId == data.Id);
                    if (studentpta != null)
                    {
                        no++;
                    }
                    var studentAddi = dbContext.AdditionalPayments.FirstOrDefault(k => k.TermRegId == data.Id);
                    if (studentpta != null)
                    {
                        no++;
                    }
                    if (no == 0)
                    {
                        var finalData = dbContext.TermRegistrations.FirstOrDefault(k => k.Id == data.Id);
                        if (finalData != null)
                        {
                            regStudents.Add(finalData);
                        }
                        //regResult
                        var results = dbContext.ResultTable.Where(k => k.TermRegId == data.Id).ToList();
                        if (results.Any())
                        {
                            foreach (var item in results)
                            {
                                regResult.Add(item);

                            }
                        }
                        var Rating = dbContext.StudentRatings.FirstOrDefault(k => k.TermRegId == data.Id);
                        if (Rating != null)
                        {
                            dbContext.Remove(Rating);
                        }
                    }
                }
                if (regStudents.Count() > 0 && regResult.Count()>0)
                {
                    dbContext.RemoveRange(regStudents, regResult);
                    int result = dbContext.SaveChanges();
                    if (result > 0)
                    {
                        if (result == 1)
                        {
                            TempData["success"] = "Student Successfully Removed From the Term";
                        }
                        else
                            TempData["success"] = "Student Successfully Removed From the Term";
                    }
                    return Page();
                }
                TempData["error"] = "We detect some records on some Students term registration";
                return Page();
            }
            TempData["error"] = "No data was sent to the server for processing. Please check and try again.";
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
        }
        public class IncomingData
        {
            public int Id { get; set; }
            public bool ischeck { get; set; }
        }
    }
}
