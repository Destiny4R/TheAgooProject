using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Path = System.IO.Path;
using static TheAgooProjectWeb.Pages.Compute_Result.Subjects_PositionModel;

namespace TheAgooProjectWeb.Pages.Compute_Result
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment hostEnvironment;
        public SettingNames settingNames;
        IExcelDataReader reader;
        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public string Disabled = "";
        public bool _generalIn = false;
        public IndexModel(ApplicationDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            this.dbContext = dbContext;
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult OnGet()
        {
            var ClaimFinder = (ClaimsIdentity)User.Identity;
            var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);
            getViewSelect();
            var settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == claim.Value);
            if(settings != null)
            {
                var generalInfo = dbContext.GeneralClassTables.FirstOrDefault(k => k.Term == settings.Term && k.SessionYearId == settings.sessionYear && k.ClassId == settings.Classes && k.SubClassId == settings.subclass);
                if (generalInfo != null)
                {
                    _generalIn = true;
                }
                settingNames = new()
                {
                    Sessionyear = dbContext.SessionYears.Find(settings.sessionYear).Name,
                    Term = settings.Term,
                    Classes = dbContext.SchoolClasses.Find(settings.Classes).Name,
                    Subclass = dbContext.SubClasses.Find(settings.subclass).Name,
                };

                return Page();
            }
            settingNames = new();
            TempData["error"] = "Please go to settings and select the class you will be working on before proceeding!";
            Disabled = "disabled";
            return Page();
        }
        public class SettingNames
        {
            public string Sessionyear { get; set; }
            public string Term { get; set; }
            public string Classes { get; set; }
            public string Subclass { get; set; }
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
        //EXCEL FILE UPLOAD HANDLER
        public async Task<IActionResult> OnPostSearch(IFormFile excelfile)
        {
            try
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
                // Check the File is received
                if (excelfile == null)
                {
                    TempData["error"] = "File is Not Received...";
                    return RedirectToPage();
                }

                // Create the Directory if it is not exist
                string dirPath = Path.Combine(hostEnvironment.WebRootPath, "ReceivedReports");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                // MAke sure that only Excel file is used 
                string dataFileName = Path.GetFileName(excelfile.FileName);

                string extension = Path.GetExtension(dataFileName);

                string[] allowedExtsnions = new string[] { ".xls", ".xlsx", "csv" };

                if (!allowedExtsnions.Contains(extension.ToLower()))
                {
                    TempData["error"] = "Sorry! This file is not allowed. Please make sure the file has one of these extensions: .xls, csv or .xlsx.";
                    return RedirectToPage();
                }

                // Make a Copy of the Posted File from the Received HTTP Request
                string saveToPath = Path.Combine(dirPath, dataFileName);

                using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
                {
					excelfile.CopyTo(stream);
                }
                int ree = 0, ess = 0;
                // Use this to handle Encodeing differences in .NET Core
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                // read the excel file
                using (var stream = new FileStream(saveToPath, FileMode.Open))
                {
                    if (extension ==".xls" || extension == ".xlsx" || extension == ".csv")
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        //Read the the Table
                        var SubjectPosition = new List<PositionInSubject>();
                        var result = new List<ResultTable>();
                        System.Data.DataTable serviceDetails = ds.Tables[0];
                        for (int i = 1; i < serviceDetails.Rows.Count; i++)
                        {
                            
                            //STUDENT REGISTRATION NUMBER
                            string studentreg = serviceDetails.Rows[i][2].ToString().Replace(" ","");
                            //STUDENT SUBJECT CODE NUMBER
                            int subjectCode = Int32.Parse(serviceDetails.Rows[i][7].ToString());
                            //Check If Student is register for the Term
                            var getuser = dbContext.TermRegistrations.Include(k => k.StudentsData).FirstOrDefault(h => h.SessionYearId == CTSSClass.SessionYear && h.ClassesInSchoolId == CTSSClass.Classes && h.SubClassId== CTSSClass.Subclass && h.Term== CTSSClass.Term && h.StudentsData.StudentRegNo== studentreg);
                            if (getuser == null)
                            {
                                var d = serviceDetails.Rows[i][2];
                                TempData["error"] = $"Error in this student registration number: {d}, or the select file information do not match your choice of selection, check and try again!";
                                return RedirectToPage();
                            }
                            //Check If Student Subject Result is already register for the Term(FORMULA)

                            double Assign = (double)serviceDetails.Rows[i][9];
                            double cwork = (double)serviceDetails.Rows[i][10];
                            double Test = (double)serviceDetails.Rows[i][11];
                            double Project = (double)serviceDetails.Rows[i][12];
                            double exam = (double)serviceDetails.Rows[i][13];
                            double Total = Assign + Test + Project + cwork + exam;

                            //RETRIVE THE STUDENT RESULT FOR UPDATE
                            var _GetResult = dbContext.ResultTable.FirstOrDefault(h => h.TermRegId==getuser.Id && h.SubjectId==subjectCode);
                            if (_GetResult != null) 
                            {
                                _GetResult.Assignment = Assign;
                                _GetResult.Test = Test;
                                _GetResult.Project = Project;
                                _GetResult.ClassWork = cwork;
                                _GetResult.Examination = exam;
                                _GetResult.Total = Total;
                                _GetResult.Grade = SD.Grade(Total);
                                _GetResult.Remark = SD.Remark(Total);
                                _GetResult.Status = true;
                                //int change = dbContext.SaveChanges();
                                result.Add(_GetResult);
                               ess++;
                            }
                            else
                            {
                                ree++;
                            }

                        }
                        if (result.Count() > 0)
                        {
                            dbContext.UpdateRange(result);
                            dbContext.SaveChanges();
                        }
                        //RETRIEVE DATA FOR UPDATING POSITION IN SUBJECT AND POSTION IN CLASS
                        var _GetResult2 = dbContext.ResultTable.Include(c => c.Termregistration.SessionYear).Include(c => c.Subjects).Include(k => k.Termregistration.StudentsData).Include(c => c.Termregistration.Schoolclasses).Include(c => c.Termregistration.SubClasses).Include(c => c.Termregistration.RemarkPositions).Include(c => c.Termregistration.StudentRatings).Where(h => h.Termregistration.SessionYearId == CTSSClass.SessionYear && h.Termregistration.ClassesInSchoolId == CTSSClass.Classes && h.Termregistration.SubClassId == CTSSClass.Subclass && h.Termregistration.Term== CTSSClass.Term).ToList();
                        
                        //COMPUTE POSITION IN SUBJECT
                        if (_GetResult2.Count() > 0)
                        {
                            SubjectPosition = new List<PositionInSubject>();
                            int succeed = 0;
                            foreach (var item in _GetResult2.GroupBy(m => m.SubjectId).ToList())
                            {

                                foreach (var item1 in item)
                                {
                                    var item2 = new PositionInSubject
                                    {
                                        ResultId = item1.Id,
                                        SubJectId = item1.SubjectId,
                                        TotalScore = (double)item1.Total,
                                        TermRegId = item1.TermRegId
                                    };
                                    SubjectPosition.Add(item2);
                                }

                            }
                            var results = new List<ResultTable>();
                            foreach (var item in SubjectPosition.GroupBy(k => k.SubJectId))
                            {
                                foreach (var item1 in item.Rank(p => p.TotalScore, (p, rank) => new PositionInSubject { TotalScore = p.TotalScore, TermRegId = p.TermRegId, Rank = rank }).ToList())
                                {
                                    var getdata = _GetResult2.FirstOrDefault(k => k.Id == item1.ResultId);
                                    if (getdata != null)
                                    {
                                        getdata.Position = item1.Rank;
                                        getdata.Status = true;
                                        results.Add(getdata);
                                    }
                                }
                            }
                            if (results.Count > 0)
                            {
                                dbContext.UpdateRange(results);
								succeed += dbContext.SaveChanges();
							}
                        }

                        //COMPUTE POSITION IN CLASS
                        if (SubjectPosition.Count() > 0)
                        {
                            //CALCULATE SCORES BASE ON INDIVIDUAL STUDENTS TOTAL SUBJECTS SCORES
                            var TermPosition = new List<PositionInClass>();
                            foreach (var item in SubjectPosition.GroupBy(l=>l.TermRegId))
                            {
                                var position = new PositionInClass();
                                foreach (var item1 in item)
                                {
                                    position.TotalScore += item1.TotalScore;
                                }
                                position.TermRegId = item.FirstOrDefault().TermRegId;
                                TermPosition.Add(position);
                            }
                            //RANK STUDENTS AND UPDATE THEIR POSITION IN CLASS
                            //RankPosition(TermPosition)studentRate
                            var remark = new List<RemarkPosition>();
                            var studentRate = new List<StudentRating>();
                            foreach (var item in TermPosition.Rank(p=>p.TotalScore, (p, rank)=>new PositionInClass { TotalScore = p.TotalScore, TermRegId = p.TermRegId, Rank=rank}).ToList())
                            {
                                var subjectsoffered = _GetResult2.Where(k => k.TermRegId == item.TermRegId).Count();
                                var ResultPost = _GetResult2.FirstOrDefault(k => k.Termregistration.RemarkPositions.TermRegId == item.TermRegId).Termregistration.RemarkPositions;
                                if (ResultPost != null)
                                {
                                    double averagecal = item.TotalScore / subjectsoffered;
                                    ResultPost.General_Remark = SD.GetComment(averagecal);
                                    ResultPost.Principal_Remark= SD.GetComment(averagecal);
                                    ResultPost.Position_In_Class = item.Rank;
                                    ResultPost.P_Status = true; 
                                    ResultPost.R_Status = true;
                                    remark.Add(ResultPost);
                                    //UPDATE STUDENT RATING
                                    var RateElective = _GetResult2.FirstOrDefault(g => g.Termregistration.StudentRatings.TermRegId == ResultPost.TermRegId).Termregistration.StudentRatings;
                                    if (RateElective != null)
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
                                            //studentRate.Add(RateElective);
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
                                            dbContext.Update(RateElective); dbContext.SaveChanges();
                                            //studentRate.Add(RateElective);
                                        }
                                        
                                    }
                                }
                            }
                            if (remark.Count() > 0 || studentRate.Count()>0)
                            {
                                try
                                {
                                    dbContext.UpdateRange(remark, studentRate);
                                    dbContext.SaveChanges();
                                }catch (Exception ex)
                                {

                                }
                            }
                        }
                        TempData["success"] = $"{ree} Not Added And {ess} Successfully Added and Subjects and Postion in Class Computed!";
                        return RedirectToPage();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Warning: One or more columns appears to contain text data or not recorded. Please ensure this data is in numeric format before uploading."+ex.Message;// + ' '+ex.Source +" "+ex.StackTrace;
            }
            //TempData["error"] = $"Something Happened Result Not Uploaded!";
            return RedirectToPage();
        }

        public class PositionInSubject
        {
            public int SubJectId { get; set; }
            public double TotalScore { get; set; }
            public int ResultId { get; set; }
            public int TermRegId { get; set; }
            public int Rank { get; set; }
        }
        public class PositionInClass
        {
            public double TotalScore { get; set; }
            public int TermRegId { get; set; }
            public int Rank { get; set; }
        }
        //METHOD THAT HANDLES POSITION IN CLASS RANKING 
    }
}
