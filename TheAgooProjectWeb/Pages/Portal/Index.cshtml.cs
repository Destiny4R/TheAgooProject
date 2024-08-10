using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Portal
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public string StudentRegNo { get; set; }

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public string Disabled = "";
        public bool _generalIn = false;
        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            getViewSelect();
        }
        public IActionResult OnPost()
        {
            getViewSelect();
			if (ModelState.IsValid)
            {
                var checkClass = dbContext.SchoolClasses.FirstOrDefault(k => k.Id == CTSSClass.Classes);
                if(checkClass != null)
                {
                    var getstudent = dbContext.StudentTable.FirstOrDefault(k=>k.StudentRegNo== StudentRegNo);
                    if(getstudent == null)
                    {
						TempData["error"] = "No student found with the provided Student Reg. Number, Check and try again!";
						return Page();
					}
                    if (checkClass.Elective)
                    {
                        return RedirectToPage("/Portal/Senior-Result-Sheet", new { session = CTSSClass.SessionYear, classes = CTSSClass.Classes, term = CTSSClass.Term, subclass = CTSSClass.Subclass, studentid = getstudent.Id });
					}
					//int session, int classes, int subclass, int studentid, string term
					return RedirectToPage("/Portal/Junior-Result-Sheet", new {session = CTSSClass.SessionYear, classes = CTSSClass.Classes, term = CTSSClass.Term, subclass= CTSSClass.Subclass, studentid = getstudent.Id});
				}
            }
            return  Page();
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
    }
}
