using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Helpers;
using ExpenseTracker.Models;
using OneOf;

namespace ExpenseTracker.Interfaces
{
    public interface IExpenseRepo
    {
        Task<List<Expense>> GetAllAsync(string userId, QueryObject query);
        Task<Expense?> CeeateAsync(int categiryId, Expense expense);
        Task<Expense?> GetByIdAsync(int id);
        Task<List<Expense>> GetByCategoryId(int catId);
        Task<Expense?> UpdateAsync(int id, Expense expense);
        Task<Expense?> DeleteAsync(int id);
        Task<Expense?> ReturnMinAmount(string userId);
        Task<Expense?> ReturnMaxAmount(string userId);
        Task<List<Expense>> GetByMonth(string userId, int monthNumber, int year);
        Task<List<Expense>> GetCurrentMonthExpenses(string userId);
    }
}