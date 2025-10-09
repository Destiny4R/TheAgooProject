using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
	public class PaymentReport
	{
		public int Id { get; set; }
		public double AccummulatedPay { get; set; }
		public string? Status { get; set; }
		public int TermRegId { get; set; }
		[ForeignKey(nameof(TermRegId))]
		public TermRegistration Termregistration { get; set; }
		public int TermlyFeesId { get; set; }
		[ForeignKey(nameof(TermlyFeesId))]
		public TermlyFeesPay TermlyFees { get; set; }
		public DateTime CreatedDate { get; set; }=DateTime.Now;

	}
}
