using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class TeamManagement
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Fullname { get; set; }
        [StringLength(60)]
        public string Position { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(11)]
        public string Phone { get; set; }
        [StringLength(2000)]
        public string Note { get; set; }
        [StringLength(1000)]
        public string ImagePath { get; set; }
        public int Hierarchy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
