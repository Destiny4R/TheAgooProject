using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class TermRegistration
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public StudentsData StudentsData { get; set; }
        public int SessionYearId { get; set; }
        [ForeignKey(nameof(SessionYearId))]
        public SessionYear SessionYear { get; set; }
        public int ClassesInSchoolId { get; set; }
        [ForeignKey(nameof(ClassesInSchoolId))]
        public SchoolClasses Schoolclasses { get; set; }
        public int SubClassId { get; set; }
        [ForeignKey(nameof(SubClassId))]
        public SubClasses SubClasses { get; set; }
        [MaxLength(20)]
        public string Term { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool ResultIsReady { get; set; } = false;
        public ICollection<ResultTable> ResultTable { get; set; }
        public StudentRating StudentRatings {  get; set; }
        public RemarkPosition RemarkPositions { get; set; }
    }
}
