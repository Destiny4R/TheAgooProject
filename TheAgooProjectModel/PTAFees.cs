using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class PTAFees
    {
        [ValidateNever]
        public int Id { get; set; }
        public double Fees { get; set; }
        public int SessionYearId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(SessionYearId))]
        public SessionYear SessionYear { get; set; }
        public string? ApplicationUserId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.Now; 
        [MaxLength(20)]
        public string Term { get; set; }
    }
}
