using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class SmsTables
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string ApplicationUserId { get; set; }
        [StringLength(12)]
        public string Sender { get; set; }
        [StringLength(250)]
        public string Subject { get; set; }
        [StringLength(1000)]
        public string Message { get; set; }
        public int SmsCount { get; set;}
        public double Cost { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
