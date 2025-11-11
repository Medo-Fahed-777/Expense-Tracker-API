using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Dtos.Category;
using ExpenseTracker.Helpers;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Mappers;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public class ReportService : IReportService
    {
        private readonly IExpenseRepo _expenseRepo;
        private readonly ILogger<ReportService> _logger;
        public ReportService(IExpenseRepo expenseRepo, ILogger<ReportService> logger)
        {
            _expenseRepo = expenseRepo;
            _logger = logger;
        }
        public async Task<MonthlyReport> GenerateMonthlyReport(string userId, int month, int year)
        {
            var expenses = await _expenseRepo.GetByMonth(userId, month, year);

            decimal totalExpenses = expenses.Sum(e => e.Ammount);

            string monthName = new DateTime(year, month, 1).ToString("MMMM");

            var topCategories = expenses
                .GroupBy(e => e.Category!)
                .Select(g => new CategoryDto
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    UserId = g.Key.UserId,
                    TotalAmount = g.Sum(e => e.Ammount)
                })
                .OrderByDescending(c => c.TotalAmount)
                .Take(1)
                .ToList();

            decimal averagePerDay = Math.Round(
                totalExpenses / DateTime.DaysInMonth(year, month), 2
            );

            return new MonthlyReport
            {
                MonthName = monthName,
                TotalExpenses = totalExpenses,
                TopCategories = topCategories, // now list of CategoryDto
                AveragePerDay = averagePerDay
            };
        }

        public async Task<SummaryReport> GenerateSummaryReport(string userId)
        {
            var query = new QueryObject();

            var expenses = await _expenseRepo.GetAllAsync(userId, query);

            var totalLifeTimeSpent = expenses.Sum(e => e.Ammount);

            var totalExpensesRecorded = expenses.Count;

            var topCategories = expenses
                .GroupBy(e => e.Category!)
                .Select(g => new CategoryDto
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    UserId = g.Key.UserId,
                    TotalAmount = g.Sum(e => e.Ammount)
                })
                .OrderByDescending(c => c.TotalAmount)
                .Take(3)
                .ToList();

            var highestSingleExpense = expenses
                .OrderByDescending(e => e.Ammount)
                .First()
                .ToDto();

            var firstExpenseDate = expenses.Min(e => e.Date).ToString("dd MMM yyyy");
            
            var mostRecentExpenseDate = expenses.Max(e => e.Date).ToString("dd MMM yyyy");
            
            return new SummaryReport
            {
                TotalLifeTimeSpent = totalLifeTimeSpent,
                TotalExpensesRecorded = totalExpensesRecorded,
                TopSpendingCategories = topCategories,
                HighestSingleExpense = highestSingleExpense,
                FirstExpenseDate = firstExpenseDate,
                MostRecentExpenseDate = mostRecentExpenseDate
            };
                
        }
    }
}