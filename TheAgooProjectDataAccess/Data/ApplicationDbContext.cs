using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectModel;

namespace TheAgooProjectDataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> k) : base(k)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<StudentsData> StudentTable { get; set; }  
        public DbSet<TermRegistration> TermRegistrations { get; set; }
        public DbSet<SessionYear> SessionYears { get; set; }
        public DbSet<SchoolClasses> SchoolClasses { get; set; }
        public DbSet<SubClasses> SubClasses { get; set; }
        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<ResultTable> ResultTable { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<GeneralClassTable> GeneralClassTables { get; set; }
        public DbSet<RemarkPosition> RemarkPositions { get; set; }
        public DbSet<StudentRating> StudentRatings { get; set; }

    }
}
