using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.Models
{
    public class FeedBack
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Display(Name="Full Name")]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Subject { get; set; }
        [Required]
        [StringLength(550)]
        [Display(Name ="Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
