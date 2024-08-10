using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TheAgooProjectModel.ViewModels
{
    public class ViewSelectModel
    {
        public IEnumerable<SelectListItem>? SessionYearz { get; set; }
        public IEnumerable<SelectListItem>? Class { get; set; }
        public IEnumerable<SelectListItem>? SubClass { get; set; }
        public IEnumerable<SelectListItem>? Subjects { get; set; }
    }
}
