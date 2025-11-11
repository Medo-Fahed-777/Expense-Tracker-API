using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Dtos.Category;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDto ToDto(this Category category)
        {
            decimal amounts = category.Expenses.Sum(e => e.Ammount);
            return new()
            {
                Id = category.Id,
                Name = category.Name,
                UserId = category.UserId,
                TotalAmount = amounts
            };
        }

        public static Category CreateDto(this CreateCategoryDto dto)
        {
            return new()
            {
                Name = dto.Name
            };
        }

        public static Category UpdateDto(this UpdateCategoryDto dto)
        {
            return new()
            {
                Name = dto.Name
            };
        }
    }
}