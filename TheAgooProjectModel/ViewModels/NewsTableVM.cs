using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
    public class NewsTableVM
    {
        public int? Id { get; set; }
        [StringLength(150)]
        [Required]
        public string Reporter { get; set; }
        [StringLength(250)]
        [Required]
        public string Headline { get; set; }
        [Required]
        public string NewBody { get; set; }
        [StringLength(150)]
        [Required]
        public string Category { get; set; }
    }
}
