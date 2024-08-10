using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;
using static TheAgooProjectWeb.Pages.Admin.TermRegistration.RegisterModel;

namespace TheAgooProjectWeb.Pages.Admin.TermRegistration
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public Input input { get; set; }
        public EditModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet(int id)
        {
            getViewSelect();
            if (id>0)
            {
                input = dbContext.TermRegistrations.Include(d=>d.StudentsData).Include(j=>j.ResultTable).ThenInclude(k=>k.Subjects).Where(k => k.Id == id).Select(h => new Input
                {
                    Fullname = h.StudentsData.SurName + " " + h.StudentsData.OtherName + " " + h.StudentsData.FirstName,
                    StudentReg = h.StudentsData.StudentRegNo,
                    StudentId = h.Id,
                    Sessionyear = h.SessionYearId,
                    Term = h.Term,
                    Classes = h.ClassesInSchoolId,
                    Subclass = h.SubClassId,
                    GetResults = h.ResultTable.ToList()
                }).FirstOrDefault();
                if (input == null)
                {
                    TempData["error"] = "We were unable to find this student in our system, are you missing something?";
                    return RedirectToPage("Index");
                }
                return Page();
            }
            TempData["error"] = "There may be a problem with your request. Can you try again?";
            return RedirectToPage("Index");
        }
        public IActionResult OnPost(List<int> subjects)
        {
            if (input.StudentId > 0)
            {
                var reg = dbContext.TermRegistrations.FirstOrDefault(l => l.Id == input.StudentId);
                if (reg != null)
                {
                    reg.Id = input.StudentId;
                    reg.SessionYearId = input.Sessionyear;
                    reg.ClassesInSchoolId = input.Classes;
                    reg.SubClassId = input.Subclass;
                    reg.Term = input.Term;
                    dbContext.Update(reg);

                    if (subjects.Count() > 0)
                    {
                        var studentsublist = new List<ResultTable>();
                        int no_subject = 0;
                        foreach (var item in subjects)
                        {
                            var checkResult = dbContext.ResultTable.FirstOrDefault(m => m.SubjectId == item && m.TermRegId == reg.Id);
                            if (checkResult == null)
                            {
                                var singledata = new ResultTable()
                                {
                                    TermRegId = reg.Id,
                                    SubjectId = item
                                };
                                studentsublist.Add(singledata);
                            }
                            else
                            {
                                no_subject++;
                            }
                        }
                        dbContext.AddRange(studentsublist);
                    }
                    int final = dbContext.SaveChanges();
                    TempData["success"] = "Student term registration updated successfully!";
                    return RedirectToPage("Index");
                }
            }
            TempData["error"] = "An error occurred while updating the student term registration information. Please try again later!";
            return RedirectToPage("Index");
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
                SessionYearz = dbContext.SessionYears.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                Subjects = dbContext.Subjects.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
        public class Input
        {
            public string Fullname { get; set; }
            public string StudentReg { get; set; }
            public int StudentId { get; set; }
            public int Sessionyear { get; set; }
            public int Classes { get; set; }
            public int Subclass { get; set; }
            public string Term { get; set; }
            public IEnumerable<ResultTable> GetResults { get; set; }
        }
    }
}
