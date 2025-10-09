using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
    public class TeamManageVM
    {
        public int? Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Fullname { get; set; }
        [StringLength(60)]
        [Required]
        public string Position { get; set; }
        [StringLength(100)]
        [Required]
        public string Email { get; set; }
        [StringLength(11)]
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Note { get; set; }
        public string? ImagePath { get; set; }
        public int Hierarchy { get; set; }
    }
}
