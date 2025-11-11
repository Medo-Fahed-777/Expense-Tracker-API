using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Dtos.Expence
{
    public class CreateExpenseDto
    {
        [Required]
        public decimal Ammount { get; set; }
        public string? Description { get; set; }

    }
}