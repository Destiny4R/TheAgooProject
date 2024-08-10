using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class Settings
    {
        public int Id { get; set; }
        public int? sessionYear { get; set; }
        [StringLength(20)]
        public string? Term { get; set; }
        public int? Classes { get; set; }
        public int? subclass { get; set; }
        [StringLength(452)]
        public string? ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public string? CashierSignature { get; set; }
        public string? PrincipalSignature { get; set; }
        public string? PrincipalName{ get; set; }
        public bool CanPrintResult { get; set; } = false;
    }
}
