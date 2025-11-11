using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Dtos.Expence;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappers
{
    public static class ExpenseMappers
    {
        public static ExpenseDto ToDto(this Expense expense)
        {
            return new()
            {
                Id = expense.Id,
                Ammount = expense.Ammount,
                Description = expense.Description,
                Date = expense.Date,
                Username = expense.User!.UserName,
                UserId = expense.UserId,
                CategoryName = expense.Category!.Name,
                CategoryId = expense.CategoryId
            };
        }

        public static Expense CreateDto(this CreateExpenseDto dto)
        {
            return new()
            {
                Ammount = dto.Ammount,
                Description = dto.Description,
                Date = DateTime.Now
            };
        }

        public static Expense UpdateDto(this UpdateExpenseDto dto)
        {
            return new()
            {
                Ammount = dto.Ammount,
                Description = dto.Description,
                CategoryId = dto.CategoryId
            };
        }
    }
}