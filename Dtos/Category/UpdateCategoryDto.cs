using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Dtos.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}