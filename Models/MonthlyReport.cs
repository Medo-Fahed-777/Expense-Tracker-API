using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Dtos.Category;

namespace ExpenseTracker.Models
{
    public class MonthlyReport
    {
        public string MonthName { get; set; } = string.Empty;
        public decimal TotalExpenses { get; set; }
        public List<CategoryDto> TopCategories { get; set; } = new();
        public decimal AveragePerDay { get; set; }
    }
}