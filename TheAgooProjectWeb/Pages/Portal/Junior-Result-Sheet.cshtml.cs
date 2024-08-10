using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Portal
{
    [Authorize]
    public class Junior_Result_SheetModel : PageModel
    {
		private readonly ApplicationDbContext dbContext;

		public int NoInClass = 0, Position = 0;
		public double ToTalScore = 0;
		public StudentsData Studentsdata { get; set; }
		public TermRegistration termReg { get; set; }
		public GeneralClassTable _generalClass { get; set; }
		public RemarkPosition _remarkPosition { get; set; }

		public Junior_Result_SheetModel(ApplicationDbContext dbContext)
        {
			this.dbContext = dbContext;
		}
		public IActionResult OnGet(int session, int classes, int subclass, int studentid, string term)
		{
			if (ModelState.IsValid)
			{
				var Results = dbContext.ResultTable.Include(k => k.Subjects).Include(k => k.Termregistration.StudentsData).Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.SubClasses).Include(k => k.Termregistration.RemarkPositions).Include(k => k.Termregistration.StudentRatings).Include(k => k.Termregistration.Schoolclasses).Where(s => s.Termregistration.SessionYearId == session && s.Termregistration.ClassesInSchoolId == classes && s.Termregistration.SubClassId == subclass && s.Termregistration.Term==term).ToList();
				if (Results.Count() < 1)
				{
					TempData["error"] = "No result found for the selection, are you missing something?";
					return RedirectToPage("Index");
				}
				
				NoInClass = Results.GroupBy(y => y.Termregistration.StudentId).Count();
				Studentsdata = dbContext.StudentTable.Find(studentid);
				termReg = Results.FirstOrDefault().Termregistration;
				_generalClass = dbContext.GeneralClassTables.Include(s => s.SessionYear).FirstOrDefault(k => k.SessionYearId == termReg.SessionYearId && k.Term == termReg.Term && k.ClassId == termReg.ClassesInSchoolId && k.SubClassId == termReg.SubClassId);
				if (_generalClass == null)
				{
					TempData["error"] = "Result still under computation, Check back later";
					return RedirectToPage("Index");
				}
				if (!termReg.RemarkPositions.P_Status)
				{
					TempData["error"] = "Result still under computation, Check back later";
					return RedirectToPage("Index");
				}
				//WHAT IS THE CURRENT POSITION OF THIS STUDENT IN CLASS
				HighPosition = new List<HighLowScore>();
				foreach (var score in Results.GroupBy(y => y.Termregistration.StudentId))
				{
					var coll = new HighLowScore();
					coll.StudentId = score.FirstOrDefault().Termregistration.StudentId;
                    foreach (var item in score)
                    {
						coll.Score += (double)item.Total;
                    }
					coll.Average = Math.Round(coll.Score / score.Count(), 2);
					HighPosition.Add(coll);
                }
				var getPosition = HighPosition.Rank(p => p.Average, (p, Rank) => new HighLowScore { Average = p.Average, Score = (double)p.Score, StudentId = p.StudentId, Rank = Rank }).ToList();
				Position = getPosition.FirstOrDefault(m => m.StudentId == studentid).Rank;
				ToTalScore = HighPosition.FirstOrDefault(k=>k.StudentId==studentid).Score;
				//Narrow THE RESULT TO A PARTICULA STUDENT
				CollectDatas = new List<CollectData>();
				foreach (var item in Results.Where(k=>k.Termregistration.StudentId==studentid))
				{
					var _highestLowest = new List<HighLowScore>();
					var collect = new CollectData() {
						SubjectId = item.SubjectId,
						SujectName= item.Subjects.Name,
						Assgmnt	= item.Assignment,
						Test = item.Test,
						CWork = item.ClassWork,
						project = item.Project,
						Exams=	item.Examination,
						TotalScores= item.Total
					};
					//GET SUBJECT AVERAGE
					var getsubject = Results.Where(j => j.SubjectId == item.SubjectId);
					collect.Highest = getsubject.Max(k => (double)k.Total);
					collect.Lowest = getsubject.Min(k => (double)k.Total);
					double gettotal = 0;
                    foreach (var item1 in getsubject)
                    {
						gettotal += (double)item1.Total;
                    }
					collect.Average = Math.Round(gettotal / getsubject.Count(),2);
					collect.Grade = SD.Grade((double)item.Total);
					collect.Remark = SD.Remark((double)item.Total);
					var getsubjec = getsubject.Rank(p => p.Total, (p, Rank) => new Averages { Score = (double)p.Total, StudentId = p.Termregistration.StudentId, Rank = Rank }).ToList();
					collect.Position = getsubjec.FirstOrDefault(k=>k.StudentId==item.Termregistration.StudentId).Rank;
					CollectDatas.Add(collect);
				}
				return Page();
			}
			return RedirectToPage("Index");
		}

		public IList<CollectData> CollectDatas { get; set; }
		public IList<HighLowScore> HighLowPosition { get; set; }
		public IList<HighLowScore> HighPosition { get; set; }
		public class CollectData
		{
			public string SujectName { get; set; }
			public int SubjectId { get; set; }
			public double? Assgmnt { get; set; }
			public double? Test { get; set; }
			public double? CWork { get; set; }
			public double? project { get; set; }
			public double? Exams { get; set; }
			public double? TotalScores { get; set; }
			public double Average { get; set; }
			public double Highest { get; set; }
			public double Lowest { get; set; }
			public int Position { get; set; }
			public string Grade { get; set; }
			public string Remark { get; set; }
		}
		public class HighLowScore
		{
			public int StudentId { get; set; }
            public double Average { get; set; }
            public double Score { get; set; }
			public int Rank { get; set; }
		}
		public class Averages
		{
			public int StudentId { get; set; }
			public int SubjId { get; set; }
			public double Score { get; set; }
			public int Rank { get; set; }
		}
	}
}
