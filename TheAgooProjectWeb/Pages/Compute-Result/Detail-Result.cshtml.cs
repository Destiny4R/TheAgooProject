using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Compute_Result
{
    [Authorize]
    public class Detail_ResultModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public IEnumerable<ResultTable> results {  get; set; }
        public Detail_ResultModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet(int id)
        {
            if (id > 0)
            {
                results = dbContext.ResultTable.Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.SubClasses).Include(k => k.Termregistration.Schoolclasses).Include(k=>k.Termregistration.StudentsData).Include(k=>k.Subjects).Where(i => i.TermRegId == id).ToList();
                if (results.Count() > 0)
                {
                    return Page();
                }
            }
            return RedirectToPage("Index");
        }
    }
}
