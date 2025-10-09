using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class Visit
    {
        [Key]
        public int Id { get; set; }
        public int Website { get; set; }
        public int Portal { get; set; }
    }
}
