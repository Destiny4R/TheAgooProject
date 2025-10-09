using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TheAgooProjectModel

{
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class ParentTable
    {
        [ValidateNever]
        public int Id { get; set; }
        [ValidateNever]
        [MaxLength(30)]
        public string StudentRegNo { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Occupation { get; set; }
        [MaxLength(30)]
        public string Relationship { get; set; }
        [MaxLength(11)]
        public string PhoneNumber { get; set; }
        [MaxLength(250)]
        public string Address { get; set; }
        [MaxLength(250)]
        public string? StaffId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [ValidateNever]
        public ParentStudent ParentStudent { get; set; }
    }
}
