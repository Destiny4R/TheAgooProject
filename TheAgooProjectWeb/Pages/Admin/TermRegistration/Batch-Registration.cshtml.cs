using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectModel.ViewModels;
using TheAgooProjectModel;
using TheAgooProjectDataAccess.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TheAgooProjectWeb.Pages.Admin.TermRegistration
{
    public class Batch_RegistrationModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        [BindProperty]
        public CTSSClass RegCtssClass { get; set; }
        [BindProperty]
        public CTSSClass ctsscl { get; set; }
        [BindProperty]
        public IList<TheAgooProjectModel.TermRegistration> TermReg { get; set; }
        public int hasValue = 0;
        public ViewSelectModel ViewSelectModel { get; set; }

        public Batch_RegistrationModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            getViewSelect();
        }
        public IActionResult OnPostSearch()
        {
            getViewSelect();
            if(CTSSClass.Classes>0 && CTSSClass.SessionYear>0 && !string.IsNullOrEmpty(CTSSClass.Term) && CTSSClass.Subclass > 0)
            {
                var termdata = dbContext.TermRegistrations.Include(x=>x.SessionYear).Include(x => x.SubClasses).Include(x => x.Schoolclasses).Include(x => x.StudentsData).Where(m => m.ClassesInSchoolId == CTSSClass.Classes && m.SessionYearId == CTSSClass.SessionYear && m.Term == CTSSClass.Term && m.SubClassId == CTSSClass.Subclass).ToList();
                if (termdata.Count > 0)
                {
                    TermReg = termdata;
                    ctsscl = new CTSSClass
                    {
                        SessionYear= CTSSClass.SessionYear,
                        Term = CTSSClass.Term,
                        Classes = CTSSClass.Classes,
                        Subclass = CTSSClass.Subclass
                    };
                    hasValue = 1;
                    TempData["success"] = "Result found for the selection made";
                    return Page();
                }
            }
            TempData["error"] = "No result found the selection made";
            return Page();
        }
        public async Task<IActionResult> OnPostPostAll()
        {
            getViewSelect();
            if (TermReg.Count() > 0)
            {
                int register = 0, notregister = 0;
                var termdata = dbContext.TermRegistrations.Include(x => x.SessionYear).Include(x => x.SubClasses).Include(x => x.Schoolclasses).Include(x => x.StudentsData).Include(x => x.ResultTable).Where(m => m.ClassesInSchoolId == ctsscl.Classes && m.SessionYearId == ctsscl.SessionYear && m.Term == ctsscl.Term && m.SubClassId == ctsscl.Subclass).ToList();
                var termregisters = new List<TheAgooProjectModel.TermRegistration>();
                foreach (var item in TermReg)
                {
                    var checkifregister = dbContext.TermRegistrations.Include(k=>k.StudentsData).FirstOrDefault(k => k.StudentsData.StudentRegNo == item.StudentsData.StudentRegNo && k.SessionYearId == RegCtssClass.SessionYear && k.ClassesInSchoolId == RegCtssClass.Classes && k.SubClassId == RegCtssClass.Subclass && k.Term == RegCtssClass.Term);
                    if (checkifregister != null)
                    {
                        notregister++;
                    }
                    else
                    {
                        var stud = dbContext.StudentTable.FirstOrDefault(j => j.StudentRegNo == item.StudentsData.StudentRegNo);
                        if (stud != null) 
                        { 
                            var termreg = new TheAgooProjectModel.TermRegistration()
                            {
                                StudentId = stud.Id,
                                SessionYearId = RegCtssClass.SessionYear,
                                ClassesInSchoolId = RegCtssClass.Classes,
                                SubClassId = RegCtssClass.Subclass,
                                Term = RegCtssClass.Term
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
                            var getOldSubjects = termdata.Where(b => b.Id == item.Id).FirstOrDefault().ResultTable.ToList();
                            foreach (var item2 in getOldSubjects)
                            {
                                var singledata = new ResultTable()
                                {
                                    TermRegId = termreg.Id,
                                    SubjectId = item2.SubjectId
                                };
                                termreg.ResultTable.Add(singledata);
                            }
                            termregisters.Add(termreg);
                            register++;
                        }
                        else
                        {
                            notregister++;
                        }
                    }
                }
                if (termregisters.Count() > 0)
                {
                    await dbContext.AddRangeAsync(termregisters);
                    await dbContext.SaveChangesAsync();
                    TempData["success"] = $"{notregister} student already registered and {register} students successfully register";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["error"] = $"{notregister} students already register for the selected information";
                    return Page();
                }
            }
            TempData["error"] = "An error occured while processing your request, try again later";
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
                })
            };
        }
        
        public class TermRegs
        {
            public string RegNumber { get; set; }
        }
    }
}
