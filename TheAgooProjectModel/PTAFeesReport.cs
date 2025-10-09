using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class PTAFeesReport
    {
        public int Id { get; set; }
        public double CulmulativePay { get; set; }
        public string Status { get; set; }
		public int TermRegId { get; set; }
		[ForeignKey(nameof(TermRegId))]
		public virtual TermRegistration Termregistration { get; set; }
        public int PTAfeesId { get; set; }
        [ForeignKey(nameof(PTAfeesId))]
        public virtual PTAFees PTAFees { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now; 
    }
}
