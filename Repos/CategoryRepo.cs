using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Data;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repos
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;

        public CategoryRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> CategoryExist(int id)
        {
            var isExist = await _context.Categories.FindAsync(id);
            if (isExist == null)
                return null;
            return isExist;
        }

        public async Task<Category?> DeleteAsync(int id)
        {
            var isExist = await _context.Categories.FindAsync(id);
            if (isExist == null)
                return null;

            _context.Categories.Remove(isExist);
            await _context.SaveChangesAsync();

            return isExist;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {

            var cat = await _context.Categories
            .Include(e => e.Expenses)
            .FirstOrDefaultAsync(c => c.Id == id);
            
            if (cat == null)
                return null;

            return cat;
        }

        public async Task<Category?> UpdateAsync(int id, Category category)
        {
            var isExist = await _context.Categories.FindAsync(id);
            if (isExist == null)
                return null;

            isExist.Name = category.Name;
            return isExist;

        }

        public async Task<List<Category>> GetAllByUserIdAsync(string userId)
        {
            var catsUser = _context.Categories
            .Where(u => u.UserId == userId)
            .Include(e => e.Expenses);
            
            var cats = await catsUser.ToListAsync();
            return cats;

        }

        public async Task<bool> HasExpenses(int id)
        {
            var hasExpenses = await _context.Expenses
            .AnyAsync(e => e.CategoryId == id);

            if (hasExpenses)
                return true;

            else return false;

        }
    }
}