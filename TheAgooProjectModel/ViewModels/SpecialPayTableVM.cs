using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
    public class SpecialPayTableVM
    {
        public int? Id { get; set; }
        public int SessionYearId { get; set; }
        public int ClassesInSchoolId { get; set; }
        [MaxLength(20)]
        public string Term { get; set; }
        public double Amount { get; set; }
        public int OtherPayId { get; set; }
    }
}
