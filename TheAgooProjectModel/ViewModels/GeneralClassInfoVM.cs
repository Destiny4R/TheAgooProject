using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
	public class GeneralClassInfoVM
	{
		public int? Id { get; set; }
		[Required]
		[MaxLength(30)]
		public string Term { get; set; }
		[Required]
		[Display(Name = "Session/Year")]
		public int SessionYearId { get; set; }
		[Required]
		[Display(Name = "Class")]
		public int ClassId { get; set; }
		[Required]
		[Display(Name = "Sub-Class")]
		public int SubClassId { get; set; }
		[Display(Name = "Next Term Fees")]
		[Required]
		public double Next_Term_Fees { get; set; }
		[Display(Name = "Term End")]
		[Required]
		public DateTime TermEnd { get; set; }
		[Display(Name = "Next Term Start")]
		[Required]
		public DateTime NextTermStart { get; set; }
		[Display(Name = "Class Teacher")]
		[Required]
		[StringLength(60)]
		public string ClassTeacher { get; set; }
		[Display(Name = "Principal's Name")]
		[MaxLength(60)]
		public string? PrincipalName { get; set; }
		[Display(Name = "Total Attendance")]
		[Required]
		public double TotalAttendance { get; set; }
	}
}
