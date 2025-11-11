using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;

namespace ExpenseTracker.Interfaces
{
    public interface IReportService
    {
    Task<MonthlyReport> GenerateMonthlyReport(string userId, int month, int year);
    Task<SummaryReport> GenerateSummaryReport(string userId);
    }
}