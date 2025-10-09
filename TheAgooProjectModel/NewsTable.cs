using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class NewsTable
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Reporter { get; set; }
        [StringLength(250)]
        public string Headline { get; set; }
        public string NewBody { get; set; }
        [StringLength(1500)]
        public string ImagePath { get; set; }
        public int Read { get; set; } = 0;
        [StringLength(150)]
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
