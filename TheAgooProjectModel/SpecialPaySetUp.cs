using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class SpecialPaySetUp
    {
        public int Id { get; set; }
        public int SessionYearId { get; set; }
        [ForeignKey(nameof(SessionYearId))]
        public SessionYear SessionYear { get; set; }
        public int ClassesInSchoolId { get; set; }
        [ForeignKey(nameof(ClassesInSchoolId))]
        public SchoolClasses Schoolclasses { get; set; }
        [MaxLength(20)]
        public string Term { get; set; }//
        public double Amount { get; set; }
        public int OtherPayId { get; set; }
        [ForeignKey(nameof(OtherPayId))]
        public OtherPayTable OtherPayTables { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
