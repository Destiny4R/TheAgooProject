using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class PTAExpenditureTable
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string ApplicationUserId { get; set;}
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public int SessionYearId { get; set; }
        [ForeignKey(nameof(SessionYearId))]
        public SessionYear SessionYear { get; set; }
        public string Term { get; set; }
        public int ExpedId { get; set; }
        [ForeignKey(nameof(ExpedId))]
        public ExpenditureTypes ExpenditureType { get; set; }
    }
}
