using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Compute_Result
{
    [Authorize]
    public class Remark_CommentModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public RemarkPosition position { get; set; }
        public double TotalScores { get; set; }
        public double TermTotalAtten { get; set; }//byte smallvalue, byte bigvalue
        [BindProperty]
        public byte smallvalue { get; set; }
        [BindProperty]
        public byte bigvalue { get; set; }
        public Remark_CommentModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet( int id)
        {
            if (id > 0)
            {
                var ClaimFinder = (ClaimsIdentity)User.Identity;
                var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);

                var settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == claim.Value);
                if (settings != null) {
                    position = dbContext.RemarkPositions.Include(c => c.Termregistration.StudentsData).FirstOrDefault(p => p.TermRegId == id);
                    
                    var result = dbContext.ResultTable.Include(c => c.Termregistration.SessionYear).Include(c => c.Subjects).Include(k => k.Termregistration.StudentsData).Include(c => c.Termregistration.Schoolclasses).Include(c => c.Termregistration.RemarkPositions).Include(c => c.Termregistration.SubClasses).Where(h => h.Termregistration.SessionYearId == settings.sessionYear && h.Termregistration.ClassesInSchoolId == settings.Classes && h.Termregistration.SubClassId == settings.subclass && h.Termregistration.Term == settings.Term && h.TermRegId==id).ToList();

                    var general = dbContext.GeneralClassTables.FirstOrDefault(h => h.SessionYearId == settings.sessionYear && h.ClassId == settings.Classes && h.SubClassId == settings.subclass && h.Term == settings.Term);

                    TermTotalAtten = general.TotalAttendance;

                    if (result.Count() > 0)
                    {
                        foreach (var item in result)
                        {
                            TotalScores += (double)item.Total;
                        }
                    }
                }
            }
        }
        public IActionResult OnPost()
        {
            if (position.Id > 0)
            {
                var remarkPosition = dbContext.RemarkPositions.Include(u=>u.Termregistration.Schoolclasses).FirstOrDefault(k=>k.Id==position.Id);

                if(remarkPosition != null)
                {
                    remarkPosition.Student_Attendance = position.Student_Attendance;
                    remarkPosition.Absent = position.Absent;
                    remarkPosition.Principal_Remark = position.Principal_Remark;
                    remarkPosition.General_Remark = position.General_Remark;
                    remarkPosition.R_Status = true;
                    dbContext.Update(remarkPosition);
                    if(bigvalue > 0 && smallvalue > 0)
                    {
                       var RateElective = dbContext.StudentRatings.FirstOrDefault(g => g.TermRegId == remarkPosition.TermRegId);
                        if (remarkPosition.Termregistration.Schoolclasses.Elective)
                        {
                            if (RateElective != null)
                            {
                                RateElective.Attentiveness = SD.Rating(smallvalue, bigvalue);
                                RateElective.Attendance = SD.Rating(smallvalue, bigvalue);
                                RateElective.Reliability = SD.Rating(smallvalue, bigvalue);
                                RateElective.Punctuality = SD.Rating(smallvalue, bigvalue);
                                RateElective.Perseverance = SD.Rating(smallvalue, bigvalue);
                                RateElective.Neatness = SD.Rating(smallvalue, bigvalue);
                                RateElective.Sense_of_Responsibility = SD.Rating(smallvalue, bigvalue);
                                RateElective.Politeness = SD.Rating(smallvalue, bigvalue);
                                RateElective.Spirit_of_Cooperation = SD.Rating(smallvalue, bigvalue);
                                RateElective.SelfControl = SD.Rating(smallvalue, bigvalue);
                                RateElective.Relationship_With_Student = SD.Rating(smallvalue, bigvalue);
                                RateElective.Relation_With_Staff = SD.Rating(smallvalue, bigvalue);
                                RateElective.Curiosity = SD.Rating(smallvalue, bigvalue);
                                RateElective.Initiative = SD.Rating(smallvalue, bigvalue);
                                RateElective.Honesty = SD.Rating(smallvalue, bigvalue);
                                RateElective.Industry = SD.Rating(smallvalue, bigvalue);
                                RateElective.Humility = SD.Rating(smallvalue, bigvalue);
                                RateElective.Organisational_Ability = SD.Rating(smallvalue, bigvalue);
                                RateElective.Tolanrance = SD.Rating(smallvalue, bigvalue);
                                RateElective.Leadership = SD.Rating(smallvalue, bigvalue);
                                RateElective.Respect_For_Other = SD.Rating(smallvalue, bigvalue);
                                RateElective.Courage = SD.Rating(smallvalue, bigvalue);
                                //psychomotor
                                RateElective.Handwriting = SD.Rating(smallvalue, bigvalue);
                                RateElective.Fluecy = SD.Rating(smallvalue, bigvalue);
                                RateElective.Drawing_Painting = SD.Rating(smallvalue, bigvalue);
                                RateElective.Games_Sport = SD.Rating(smallvalue, bigvalue);
                                RateElective.Handing_WShop_Tool = SD.Rating(smallvalue, bigvalue);
                                RateElective.Musical_Skill = SD.Rating(smallvalue, bigvalue);
                                RateElective.Constrution = SD.Rating(smallvalue, bigvalue);
                                dbContext.Update(RateElective);
                            }
                            else
                            {
                                RateElective.Attentiveness = SD.Rating(smallvalue, bigvalue);
                                RateElective.Attendance = SD.Rating(smallvalue, bigvalue);
                                RateElective.Punctuality = SD.Rating(smallvalue, bigvalue);
                                RateElective.Neatness = SD.Rating(smallvalue, bigvalue);
                                RateElective.Politeness = SD.Rating(smallvalue, bigvalue);
                                RateElective.SelfControl = SD.Rating(smallvalue, bigvalue);
                                RateElective.Relationship_With_Student = SD.Rating(smallvalue, bigvalue);
                                RateElective.Curiosity = SD.Rating(smallvalue, bigvalue);
                                RateElective.Honesty = SD.Rating(smallvalue, bigvalue);
                                RateElective.Humility = SD.Rating(smallvalue, bigvalue);
                                RateElective.Tolanrance = SD.Rating(smallvalue, bigvalue);
                                RateElective.Leadership = SD.Rating(smallvalue, bigvalue);
                                RateElective.Courage = SD.Rating(smallvalue, bigvalue);
                                //psychomotor
                                RateElective.Handwriting = SD.Rating(smallvalue, bigvalue);
                                RateElective.Fluecy = SD.Rating(smallvalue, bigvalue);
                                RateElective.Games_Sport = SD.Rating(smallvalue, bigvalue);
                                RateElective.Musical_Skill = SD.Rating(smallvalue, bigvalue);
                                RateElective.Constrution = SD.Rating(smallvalue, bigvalue);
                                dbContext.Update(RateElective);
                            }
                            
                        }
                    }
                    int result = dbContext.SaveChanges();
                    if (result > 0)
                    {
                        TempData["success"] = "Student Remarks, Affective development and Psychomotor skills successfully Updated";
                        return RedirectToPage("Index");
                    }
                }
                TempData["error"] = "We couldn't identify this record, please again later.";
                return RedirectToPage();
            }
            TempData["error"] = "An error occured while updating this record, please again later.";
            return RedirectToPage();
        }

    }
}
