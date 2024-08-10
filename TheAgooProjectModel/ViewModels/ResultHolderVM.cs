using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
    public class ResultHolderVM
    {
        public int Id {  get; set; }
        public double? Assignment { get; set; }
        public double? Test { get; set; }
        public double? Project { get; set; }
        public double? ClassWork { get; set; }
        public double? Examination { get; set; }
        public double? Total { get; set; }
        public string Grade { get; set; }
        public string Remark { get; set; }
        public bool Status { get; set; }
        public int TermRegId { get; set; }
        public int SubjectsCode { get; set; }
        [ValidateNever]
        public TermRegistration Termregistration { get; set; }
    }
}
