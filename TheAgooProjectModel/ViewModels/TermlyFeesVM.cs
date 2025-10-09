using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TheAgooProjectModel.ViewModels
{
    public class TermlyFeesVM
    {
        [ValidateNever]
        public int Id { get; set; }
        public double Amount { get; set; }
        [Required]
        [Display(Name = "Session/Year")]
        public int SessionYearId { get; set; }
        [Required]
        [Display(Name ="Class")]
        public int ClassesInSchoolId { get; set; }
        [MaxLength(20)]
        [Required]
        public string Term { get; set; }
    }
}
