using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.student
{
    [Authorize(Roles = SD.IsStudent)]
    public class profileModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public profileModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public StudentsData Student { get; set; } = default!;
        public List<ParentRecord> Parents { get; set; } = new();
        public EnrollmentRecord? LatestEnrollment { get; set; }
        public int TotalEnrollments { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            var student = await dbContext.StudentTable.AsNoTracking().FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);
            if (student == null)
            {
                TempData["error"] = "We could not find your student record. Please contact the school administrator.";
                return RedirectToPage("index");
            }
            Student = student;

            Parents = await dbContext.ParentStudents
                .AsNoTracking()
                .Include(ps => ps.ParentTable)
                .Where(ps => ps.StudentsdataId == student.Id)
                .Select(ps => new ParentRecord
                {
                    Name = ps.ParentTable.Name,
                    Occupation = ps.ParentTable.Occupation,
                    Relationship = ps.ParentTable.Relationship,
                    PhoneNumber = ps.ParentTable.PhoneNumber,
                    Address = ps.ParentTable.Address
                })
                .ToListAsync();

            var enrollments = await dbContext.TermRegistrations
                .AsNoTracking()
                .Include(t => t.SessionYear)
                .Include(t => t.Schoolclasses)
                .Include(t => t.SubClasses)
                .Where(t => t.StudentId == student.Id)
                .OrderByDescending(t => t.SessionYear.Name)
                .ThenByDescending(t => t.Term)
                .ToListAsync();

            TotalEnrollments = enrollments.Count;

            var latest = enrollments.FirstOrDefault();
            if (latest != null)
            {
                LatestEnrollment = new EnrollmentRecord
                {
                    Session = latest.SessionYear.Name,
                    Term = latest.Term,
                    ClassName = latest.Schoolclasses.Name,
                    SubClassName = latest.SubClasses.Name,
                    DateRegistered = latest.CreatedDate
                };
            }

            return Page();
        }

        public class ParentRecord
        {
            public string Name { get; set; } = string.Empty;
            public string Occupation { get; set; } = string.Empty;
            public string Relationship { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
        }

        public class EnrollmentRecord
        {
            public string Session { get; set; } = string.Empty;
            public string Term { get; set; } = string.Empty;
            public string ClassName { get; set; } = string.Empty;
            public string SubClassName { get; set; } = string.Empty;
            public DateTime DateRegistered { get; set; }
        }
    }
}
