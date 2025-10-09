using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class TermlyFeesPay
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int SessionYearId { get; set; }
        [ForeignKey(nameof(SessionYearId))]
        public SessionYear SessionYear { get; set; }
        public int ClassesInSchoolId { get; set; }
        [ForeignKey(nameof(ClassesInSchoolId))]
        public SchoolClasses Schoolclasses { get; set; }
        [MaxLength(20)]
        public string Term { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
