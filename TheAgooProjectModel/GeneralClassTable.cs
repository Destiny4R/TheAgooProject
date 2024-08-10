using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TheAgooProjectModel
{
	public class GeneralClassTable
	{
		public int Id { get; set; }
		[Required]
		[ValidateNever]
		public string Term { get; set; }
		[Required]
		[Display(Name = "Session/Year")]
		public int SessionYearId { get; set; }
		[ForeignKey(nameof(SessionYearId))]
		public SessionYear SessionYear { get; set; }
		[Required]
		[Display(Name = "Class")]
		public int ClassId { get; set; }
		[ForeignKey(nameof(ClassId))]
		public SchoolClasses Classcchool { get; set; }
		[Required]
		[Display(Name = "Sub-Class")]
		public int SubClassId { get; set; }
		[ForeignKey(nameof(SubClassId))]
		public SubClasses SubClasses { get; set; }
		[Display(Name = "Next Term Fees")]
		[Required]
		public double Next_Term_Fees { get; set; }
		[Display(Name = "Next Term End")]
		[Required]
		public DateTime TermEnd { get; set; }
		[Display(Name = "Next Term Start")]
		[Required]
		public DateTime NextTermStart { get; set; }
		[Display(Name = "Class Teacher")]
		[Required]
        [StringLength(60)]
        public string ClassTeacher { get; set; }
		[Display(Name = "Total Attendance")]
		[Required]
		public double TotalAttendance { get; set; }
		[ValidateNever]
		public string? ExamOfficerID { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
	}
}
