using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class ExpenditureFiles
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string? Name { get; set; }
        [StringLength(250)]
        public string filePath { get; set; }
        public int ExpenditureId {  get; set; }
        [ForeignKey(nameof(ExpenditureId))]
        public ExpenditureTable ExpenditureTable { get; set;}
        public bool AccountClose { get; set; } = false;
    }
}
