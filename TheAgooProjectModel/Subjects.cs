using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class Subjects
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        public bool Register_State { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
