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
    public class StudentDataVM
    {
        [ValidateNever]
        public int Id { get; set; }
        [ValidateNever]
        public string? StudentRegNo { get; set; }
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [Display(Name = "Other Name")]
        public string? OtherName { get; set; }
        [MaxLength(50)]
        [Display(Name = "Surname")]
        public string SurName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Gender { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Address { get; set; }
        [Required]
        [Display(Name ="Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        [Display(Name = "Local Government")]
        public string LocalGov { get; set; }
        [ValidateNever]
        public string Passport { get; set; }
        [Display(Name = "Session/Year")]
        public int? SessionYearId { get; set; }
        [ValidateNever]
        public string Password { get; set; }
    }
}
