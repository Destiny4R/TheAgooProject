using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using static TheAgooProjectWeb.Pages.Compute_Result.IndexModel;

namespace TheAgooProjectWeb.Pages.Compute_Result
{
    [Authorize]
    public class Position_ClassModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public SettingNames settingNames;
        public int DataTrue = 0;
        public IEnumerable<ResultTable> ResultTables { get; set; }
        public Position_ClassModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet()
        {
            var ClaimFinder = (ClaimsIdentity)User.Identity;
            var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);

            var settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == claim.Value);
            
            settingNames = new()
            {
                Sessionyear = dbContext.SessionYears.Find(settings.sessionYear).Name,
                Term = settings.Term,
                Classes = dbContext.SchoolClasses.Find(settings.Classes).Name,
                Subclass = dbContext.SubClasses.Find(settings.subclass).Name,
            };
            var _GetResult2 = dbContext.ResultTable.Include(c => c.Termregistration.SessionYear).Include(c => c.Subjects).Include(k => k.Termregistration.StudentsData).Include(c => c.Termregistration.Schoolclasses).Include(c => c.Termregistration.RemarkPositions).Include(c => c.Termregistration.SubClasses).Where(h => h.Termregistration.SessionYearId == settings.sessionYear && h.Termregistration.ClassesInSchoolId == settings.Classes && h.Termregistration.SubClassId == settings.subclass && h.Termregistration.Term == settings.Term).ToList();
            if (_GetResult2 != null)
            {
                DataTrue = 2;
                ResultTables = _GetResult2;
                return Page();
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            var ClaimFinder = (ClaimsIdentity)User.Identity;
            var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);

            var settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == claim.Value);

            var _GetResult2 = dbContext.ResultTable.Include(c => c.Termregistration.SessionYear).Include(c => c.Subjects).Include(k => k.Termregistration.StudentsData).Include(c => c.Termregistration.Schoolclasses).Include(c => c.Termregistration.RemarkPositions).Include(c => c.Termregistration.SubClasses).Where(h => h.Termregistration.SessionYearId == settings.sessionYear && h.Termregistration.ClassesInSchoolId == settings.Classes && h.Termregistration.SubClassId == settings.subclass && h.Termregistration.Term == settings.Term).ToList();
            if (_GetResult2 != null)
            {
                DataTrue = 2;
                var postionX = new List<PostionZ>();
                foreach (var item in _GetResult2.GroupBy(k => k.TermRegId))
                {
                    var post = new PostionZ()
                    {
                        TermRegId = item.FirstOrDefault().TermRegId
                    };
                    foreach (var item1 in item)
                    {
                        post.TotalScores += (double)item1.Total;
                    }
                    postionX.Add(post);
                }
                var remark = new List<RemarkPosition>();
                var studentRate = new List<StudentRating>();
                foreach (var item in postionX.Rank(p => p.TotalScores, (p, rank) => new PostionZ { TotalScores = p.TotalScores, TermRegId = p.TermRegId, Rank = rank }).ToList())
                {
                    var ResultPost = dbContext.RemarkPositions.FirstOrDefault(k => k.TermRegId == item.TermRegId);
                    if (ResultPost != null)
                    {
                        var subjectsoffered = _GetResult2.Where(k => k.TermRegId == item.TermRegId).Count();
                        double averagecal = item.TotalScores / subjectsoffered;
                        ResultPost.Position_In_Class = item.Rank;
                        ResultPost.General_Remark = SD.GetComment(averagecal);
                        ResultPost.Principal_Remark = SD.GetComment(averagecal);
                        ResultPost.P_Status = true;
                        ResultPost.R_Status = true;
                        remark.Add(ResultPost);
                        var RateElective = dbContext.StudentRatings.FirstOrDefault(g => g.TermRegId == ResultPost.TermRegId);
                        if (RateElective !=null)
                        {
                            if (ResultPost.Termregistration.Schoolclasses.Elective)
                            {
                                RateElective.Attentiveness = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Attendance = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Reliability = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Punctuality = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Perseverance = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Neatness = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Sense_of_Responsibility = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Politeness = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Spirit_of_Cooperation = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.SelfControl = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Relationship_With_Student = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Relation_With_Staff = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Curiosity = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Initiative = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Honesty = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Industry = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Humility = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Organisational_Ability = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Tolanrance = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Leadership = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Respect_For_Other = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Courage = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                //psychomotor
                                RateElective.Handwriting = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Fluecy = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Drawing_Painting = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Games_Sport = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Handing_WShop_Tool = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Musical_Skill = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Constrution = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                dbContext.Update(RateElective); dbContext.SaveChanges();
                            }
                            else
                            {
                                RateElective.Attentiveness = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Attendance = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Punctuality = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Neatness = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Politeness = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.SelfControl = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Relationship_With_Student = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Curiosity = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Honesty = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Humility = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Tolanrance = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Leadership = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Courage = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                //psychomotor
                                RateElective.Handwriting = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Fluecy = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Games_Sport = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Musical_Skill = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                RateElective.Constrution = SD.Rating(SD.LowRating(averagecal), SD.HighRating(averagecal));
                                dbContext.Update(RateElective);
                                dbContext.SaveChanges();
                            }
                        }
                        
                    }
                }
                if (remark.Count() > 0 || studentRate.Count() > 0)
                {
                    dbContext.UpdateRange(remark, studentRate);
                    dbContext.SaveChanges();
                    DataTrue = 2;
                    ResultTables = _GetResult2;
                    TempData["success"] = "Students postions in Class has been successfully computed and updated!";
                    return RedirectToPage();
                }
            }
            TempData["error"] = "“Sorry, we couldn’t compute the position in class at the moment. Please try again later.”";
            return RedirectToPage();
        }
        public List<PostionZ> postions { get; set; }
        public IList<PostionData> positionData { get; set; }
        public class PostionData
        {
            public string Fullname { get; set; }
            public string StudentReg { get; set; }
            public string Term { get; set; }
            public string SessionYear { get; set; }
            public string Classes { get; set; }
            public string Subclass { get; set; }
            public double TotalScores { get; set; }
        }
        public class PostionZ
        {
            public int TermRegId { get; set; }
            public double TotalScores { get; set; }
            public int Rank { get; set; }
        }
    }
}
