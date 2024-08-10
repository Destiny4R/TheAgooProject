
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO.Packaging;
using System.Net;
using System.Reflection;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;
using static TheAgooProjectWeb.Pages.Compute_Result.AssessmentModel;

namespace TheAgooProjectWeb.Pages.Compute_Result
{
    [Authorize]
    public class AssessmentModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment environment;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public string namesheet = "";
        public bool IsElective = false;
        public IEnumerable<ResultTable> results { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public AssessmentModel(ApplicationDbContext dbContext, IWebHostEnvironment _environment)
        {
            this.dbContext = dbContext;
            environment = _environment;
        }
        public void OnGet()
        {
            getViewSelect();
            results = new Collection<ResultTable>();
        }
        public IActionResult OnPost()
        {
            getViewSelect();
            if (ModelState.IsValid)
            {
                results = dbContext.ResultTable.Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.SubClasses).Include(k => k.Termregistration.Schoolclasses).Include(k => k.Termregistration.StudentsData).Include(k => k.Subjects).Where(i => i.Termregistration.SessionYearId == CTSSClass.SessionYear && i.Termregistration.Term ==CTSSClass.Term && i.Termregistration.ClassesInSchoolId==CTSSClass.Classes && i.Termregistration.SubClassId==CTSSClass.Subclass).ToList();
                if(results.Count() < 1)
                {
                    results = new Collection<ResultTable>();
                    TempData["error"] = "We couldn't find any record that matches your choice of selection";
                }
                var b = results.FirstOrDefault();
                namesheet = b.Termregistration.Term + "_" + b.Termregistration.SessionYear.Name.Replace('/','_') + "_" + b.Termregistration.Schoolclasses.Name.Replace(" ","") + "_" + b.Termregistration.SubClasses.Name;

                return Page();
            }
            results = new Collection<ResultTable>();
            TempData["error"] = "We couldn't find any record that matches your choice of selection";
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
        public class AssessmentSheet
        {
            public int SN { get; set; }
            public double Assignment { get; set; }
            public double Test { get; set; }
            public double Project { get; set; }
            public double ClassWork { get; set; }
            public double Examination { get; set; }
            public string Fullname { get; set; }
            public string StudentReg { get; set; }
            public int SubjectCode { get; set; }
            public string SubjectName { get; set; }
            public string Term { get; set; }
            public int SessionCode { get; set; }
            public int ClassCode { get; set; }
            public int SubClassCode { get; set; }
        }

    }
}
