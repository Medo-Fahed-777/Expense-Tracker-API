using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Ammount { get; set; }
        public string? Description { get; set; }
        public AppUser? User { get; set; }
        public string UserId { get; set; } = string.Empty;
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public DateTime Date { get; set; } 
    }
}