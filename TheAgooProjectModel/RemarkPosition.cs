using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class RemarkPosition
    {
        public int Id { get; set; }
        public double? Student_Attendance { get; set; }
        [MaxLength(10)]
        public string? Absent { get; set; } = null;
        public double? Total_Marks_Obtain { get; set; } 
        public int? Position_In_Class { get; set; }
        [MaxLength(250)]
        public string? General_Remark { get; set; }
        [MaxLength(250)]
        public string? Principal_Remark { get; set; }
        [MaxLength(250)]
        public string? AddedBy { get; set; }
        public bool P_Status { get; set; } = false;
        public bool R_Status { get; set; } = false;
        public int TermRegId { get; set; }
        [ForeignKey(nameof(TermRegId))]
        public TermRegistration Termregistration { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
