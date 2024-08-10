using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class SchoolClasses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Elective { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
