using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
    public class CTSSClass
    {
        [Display(Name = "Session/Year")]
        public int SessionYear { get; set; }
        [Display(Name = "Class")]
        public int Classes { get; set; }
        [Display(Name = "Sub-class")]
        public int Subclass { get; set; }
        public string Term { get; set; }
    }
}
