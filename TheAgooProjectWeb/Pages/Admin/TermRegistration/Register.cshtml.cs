using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Admin.TermRegistration
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        
        public List<int> subject = new List<int>();
        public int verify = 0;
        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public Input input { get; set; }

        public RegisterModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            input = new();
            getViewSelect();
        }
        public IActionResult OnPostSearch(string regnumber)
        {
            getViewSelect();
            input = new();
            if (!string.IsNullOrEmpty(regnumber))
            {
                input = dbContext.StudentTable.Where(k => k.StudentRegNo == regnumber).Select(h=> new Input
                {
                    Fullname = h.SurName+" "+h.OtherName+" "+h.FirstName,
                    StudentReg = h.StudentRegNo,
                    StudentId = h.Id
                }).FirstOrDefault();
                if (input == null)
                {
                    TempData["error"] = "No student found with the provided registration number";
                    return Page();
                }
                verify = 1;
                TempData["success"] = "Student found!";
                return Page();
            }
            TempData["error"] = "Provide a student registration number";
            return Page();
        }
        public IActionResult OnPostRegister(List<int> subjects)
        {
            getViewSelect();
            if (subjects.Count > 0 && input.StudentId>0)
            {
                var checkifregister = dbContext.TermRegistrations.FirstOrDefault(k => k.StudentId == input.StudentId && k.SessionYearId == CTSSClass.SessionYear && k.ClassesInSchoolId == CTSSClass.Classes && k.SubClassId == CTSSClass.Subclass && k.Term == CTSSClass.Term);
                if (checkifregister != null)
                {
                    TempData["error"] = "Student already registered for the select term, check and try again";
                    return Page();
                }
                var termreg = new TheAgooProjectModel.TermRegistration()
                {
                    StudentId = input.StudentId,
                    SessionYearId = CTSSClass.SessionYear,
                    ClassesInSchoolId = CTSSClass.Classes,
                    SubClassId = CTSSClass.Subclass,
                    Term = CTSSClass.Term
                };
                termreg.RemarkPositions = new RemarkPosition
                {
                    TermRegId = termreg.Id
                };
                termreg.StudentRatings = new StudentRating()
                {
                    TermRegId = termreg.Id
                };
                termreg.ResultTable = new List<ResultTable>();
                    
                foreach (var item in subjects)
                {
                    var singledata = new ResultTable() { 
                    TermRegId = termreg.Id,
                    SubjectId = item
                    };
                    termreg.ResultTable.Add(singledata);
                }
                dbContext.Add(termreg); 
                int final = dbContext.SaveChanges();
                TempData["success"] = "Student successfully registered for the term!";
                return RedirectToPage("Index");

            }
            input = new();
            TempData["error"] = "Provide a student registration number and search";
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
        }
    }
}
