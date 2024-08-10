using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
    public class ApplicationUserVM
    {
        [MaxLength(150)]
        public string Fullname { get; set; }
        [MaxLength(80)]
        public string Position { get; set; }
    }
}
