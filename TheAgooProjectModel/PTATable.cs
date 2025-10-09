using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAgooProjectModel;

namespace TheAgooProjectModel
{
    public class PTATable
    {
        public int Id { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public double Balance { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }=DateTime.Now; 
        [Required]
        public int TermRegId { get; set; }
        [ForeignKey(nameof(TermRegId))]
        public TermRegistration Termregistration { get; set; }
        [Required]
        public int PTAFeesId { get; set; }
        [ForeignKey(nameof(PTAFeesId))]
        public PTAFees PTAFee { get; set; }
        public string ReceiptNo { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
