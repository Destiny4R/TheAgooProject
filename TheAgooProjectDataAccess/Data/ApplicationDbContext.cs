using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectModel;
using TheAgooProjectModel.Models;

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
        public DbSet<Payment> Payments {  get; set; }
        public DbSet<PaymentReport> PaymentReports { get; set; }
        public DbSet<TermlyFeesPay> TermLyFeesPays { get; set; }
        public DbSet<ExpenditureTable> ExpenditureTables { get; set; }
        public DbSet<ExpenditureFiles> ExpenditureFiles { get; set; }
        public DbSet<ExpenditureTypes> ExpenditureType { get; set; }
        public DbSet<SmsTables> SmsTable { get; set; }
        public DbSet<ParentTable> ParentTables { get; set; }
        public DbSet<ParentStudent> ParentStudents { get; set; }
        public DbSet<PTAFees> PTAfees { get; set; }
        public DbSet<PTATable> PTATables { get; set; }
        public DbSet<PTAFeesReport> PTAFeesReports { get; set; }
        public DbSet<PTAExpenditureTable> PTAExpenditureTables { get; set; }
        public DbSet<Visit> Visitors { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<OtherPayTable> OtherPayTables { get; set; }
        public DbSet<AdditionalPayment> AdditionalPayments { get; set; }
        public DbSet<SpecialPaySetUp> SpecialPaySetUps { get; set; }
        public DbSet<NewsTable> NewsTables { get; set; }
        public DbSet<GalleryTable> GalleryTables { get; set; }
        public DbSet<ImagesTable> ImagesTables { get; set; }
        public DbSet<TeamManagement> TeamManagements { get; set; }
    }
}
