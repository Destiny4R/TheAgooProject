using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
    public class ExpenseVM
    {
        [ValidateNever]
        public int? Id { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public int ExpendId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int SessionYearId { get; set; }
        public string Term { get; set; }
    }
}
