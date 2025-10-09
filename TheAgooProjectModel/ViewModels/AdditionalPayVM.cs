using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
    public class AdditionalPayVM
    {
        public double Amount { get; set; }
        public int TermRegId { get; set; }
        public int OtherId { get; set; }

    }
}
