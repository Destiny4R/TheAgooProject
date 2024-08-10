using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class ResultTable
    {
        public int Id { get; set; }
        public double? Assignment { get; set; }
        public double? Test { get; set; }
        public double? Project { get; set; }//
        public double? ClassWork { get; set; }
        public double? Examination { get; set; }
        public double? Total { get; set; }
        public int? Position { get; set; }
        [MaxLength(4)]
        public string? Grade { get; set; }
        [MaxLength(20)]
        public string? Remark { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subjects Subjects { get; set; }
        public int TermRegId { get; set; }
        [ForeignKey(nameof(TermRegId))]
        public TermRegistration Termregistration { get; set; }
        public string? ExamsOfficer { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool Status { get; set; } = false;
    }
}
