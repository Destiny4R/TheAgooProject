using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class AdditionalPayment
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int TermRegId { get; set; }
        [ForeignKey(nameof(TermRegId))]
        public TermRegistration Termregistration { get; set; }
        [StringLength(450)]
        public string? CasheirId { get; set; }
        [ForeignKey(nameof(CasheirId))]
        public ApplicationUser ApplicationUser { get; set; }
        public int SpecialpayId { get; set; }
        [ForeignKey(nameof(SpecialpayId))]
        public SpecialPaySetUp SpecialPay { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    }
}
