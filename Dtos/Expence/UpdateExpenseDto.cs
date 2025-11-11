using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Dtos.Expence
{
    public class UpdateExpenseDto
    {
        [Required]
        public decimal Ammount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; } 
    }
}