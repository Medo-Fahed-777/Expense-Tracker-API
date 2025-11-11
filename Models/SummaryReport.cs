using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Dtos.Category;
using ExpenseTracker.Dtos.Expence;

namespace ExpenseTracker.Models
{
    public class SummaryReport
    {
        public decimal TotalLifeTimeSpent { get; set; }
        public int TotalExpensesRecorded { get; set; }
        public List<CategoryDto> TopSpendingCategories { get; set; } = new();
        public ExpenseDto HighestSingleExpense { get; set; } = new();
        public string FirstExpenseDate { get; set; } = string.Empty;
        public string MostRecentExpenseDate { get; set; } = string.Empty;
    }
}