using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Data;
using ExpenseTracker.Helpers;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace ExpenseTracker.Repos
{
    public class ExpenseRepo : IExpenseRepo
    {
        private readonly AppDbContext _context;
        public ExpenseRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Expense?> CeeateAsync(int categiryId, Expense expense)
        {
            var categoryExist = await _context.Categories.FindAsync(categiryId);
            if (categoryExist == null)
                return null;

            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            return expense;
        }

        public async Task<Expense?> DeleteAsync(int id)
        {
            var isExist = await _context.Expenses.FindAsync(id);
            if (isExist == null)
                return null;

            _context.Expenses.Remove(isExist);
            await _context.SaveChangesAsync();

            return isExist;
        }

        public async Task<List<Expense>> GetAllAsync(string userId, QueryObject? query = null)
        {
            var expenses = _context.Expenses
                .Where(e => e.UserId == userId)
                .Include(e => e.Category)
                .ThenInclude(c => c!.User)
                .AsQueryable();

            if (query != null)
            {
                // ✅ Filter by category name (if provided)
                if (!string.IsNullOrWhiteSpace(query.CategoryName))
                {
                    var filter = query.CategoryName.ToLower();
                    expenses = expenses.Where(e =>
                        e.Category != null &&
                        e.Category.Name.Contains(filter));
                }

                // ✅ Sorting logic 
                if (!string.IsNullOrWhiteSpace(query.SortBy))
                {
                    switch (query.SortBy.ToLower())
                    {
                        case "categoryname":
                            expenses = query.IsDecending
                                ? expenses.OrderByDescending(e => e.Category!.Name)
                                : expenses.OrderBy(e => e.Category!.Name);
                            break;

                        case "amount":
                            expenses = query.IsDecending
                                ? expenses.OrderByDescending(e => e.Ammount)
                                : expenses.OrderBy(e => e.Ammount);
                            break;

                        default:
                            expenses = expenses.OrderByDescending(e => e.Id);
                            break;
                    }
                }
                // ✅ Filter by Month Number (if provided)
                if (query.MonthNumber >= 1 && query.MonthNumber <= 12)
                {
                    expenses = expenses.Where(e => e.Date.Month == query.MonthNumber);
                }
                else
                {
                    expenses = expenses.OrderByDescending(e => e.Id);
                }
                // ✅ Pagination (optional)
                if (query.PageNumber > 0 && query.PageSize > 0)
                {
                    var skip = (query.PageNumber - 1) * query.PageSize;
                    expenses = expenses.Skip(skip).Take(query.PageSize);
                }
            }

            return await expenses.ToListAsync();
        }

        public async Task<Expense?> UpdateAsync(int id, Expense expense)
        {
            var isExist = await _context.Expenses
            .Include(c => c.Category)
            .ThenInclude(u => u!.User)
            .FirstOrDefaultAsync( e => e.Id == id );

            if (isExist == null)
                return null;

            isExist.Ammount = expense.Ammount;
            isExist.Description = expense.Description;

            if (isExist.CategoryId != expense.CategoryId)
            {
                var catExist = await _context.Categories
                .AnyAsync(c => c.Id == expense.CategoryId);

                if (!catExist)
                    throw new Exception("Category not found");

                isExist.CategoryId = expense.CategoryId;
            }
            
            await _context.SaveChangesAsync();

            return isExist;
        }

        public async Task<Expense?> GetByIdAsync(int id)
        {
            var expense = await _context.Expenses
            .Where(e => e.Id == id)
            .Include(c => c.Category)
            .ThenInclude(u => u!.User)
            .FirstOrDefaultAsync();

            return expense;
        }

        public async Task<List<Expense>> GetByCategoryId(int catId)
        {
            var expenses = await _context.Expenses
            .Where(c => c.CategoryId == catId)
            .Include(c => c.Category)
            .ThenInclude(u => u!.User)
            .ToListAsync();

            return expenses;
        }

        public async Task<Expense?> ReturnMinAmount(string userId)
        {
            var expenses = await _context.Expenses
            .Where(u => u.UserId == userId)
            .Include(c => c.Category)
            .ThenInclude(u => u!.User)
            .ToListAsync();

            if (expenses == null || expenses.Count == 0)
                return null;

            var minExpense = expenses.OrderBy(e => e.Ammount).First();
            return minExpense;

        }

        public async Task<Expense?> ReturnMaxAmount(string userId)
        {
            var expenses = await _context.Expenses
            .Where(u => u.UserId == userId)
            .Include(c => c.Category)
            .ThenInclude(u => u!.User)
            .ToListAsync();

            if (expenses == null || expenses.Count == 0)
                return null;

            var maxExpense = expenses.OrderByDescending(e => e.Ammount).First();
            return maxExpense;
        }

        public async Task<List<Expense>> GetByMonth(string userId, int monthNumber, int year)
        {
            if (monthNumber == 0)
            {
                return new List<Expense>();
            }
            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId &&
                 e.Date.Month == monthNumber && e.Date.Year == year)
                .Include(e => e.Category)
                .ThenInclude(c => c!.User)
                .ToListAsync();
            return expenses;
        }

        public async Task<List<Expense>> GetCurrentMonthExpenses(string userId)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId &&
                 e.Date.Month == currentMonth && e.Date.Year == currentYear)
                .Include(e => e.Category)
                .ThenInclude(c => c!.User)
                .ToListAsync();
                
            return expenses;
        }
    }
}