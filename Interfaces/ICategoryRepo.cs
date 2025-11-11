using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;

namespace ExpenseTracker.Interfaces
{
    public interface ICategoryRepo
    {
        Task<Category> CreateAsync(Category category);
        Task<Category?> GetByIdAsync(int id);
        Task<List<Category>> GetAllByUserIdAsync(string userId);
        Task<Category?> UpdateAsync(int id, Category category);
        Task<Category?> CategoryExist(int id);
        Task<Category?> DeleteAsync(int id);
        Task<bool> HasExpenses(int id);

        

    }
}