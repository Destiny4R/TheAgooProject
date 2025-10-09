using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class Payment
    {
        public int Id { get; set; }
        [Required]
        public double Paid { get; set; }
        public double Balance { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        [MaxLength(20)]
        public string? Status { get; set; }
        [Required]
        public int TermlyFeesId { get; set; }
        [ForeignKey(nameof(TermlyFeesId))]
        public TermlyFeesPay TermlyFees { get; set; }
        [Required]
        public int TermRegId { get; set; }
        [ForeignKey(nameof(TermRegId))]
        public TermRegistration Termregistrations { get; set; }
        [MaxLength(100)]
        public string ReceiptNo { get; set; }
        [MaxLength(100)]
        public string? Cashier { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
    }
}
