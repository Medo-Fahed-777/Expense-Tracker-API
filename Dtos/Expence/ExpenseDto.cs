using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;

namespace ExpenseTracker.Dtos.Expence
{
    public class ExpenseDto
    {
        
        public int Id { get; set; }
        public decimal Ammount { get; set; }
        public string? Description { get; set; }
        public string? Username { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? CategoryName { get; set; } 
        public int CategoryId { get; set; }
        public DateTime Date { get; set; } 
    }
}