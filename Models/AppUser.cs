using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Models
{
    public class AppUser : IdentityUser
    {
        public List<Category> Categories { get; set; } = new();
        public List<Expense> Expenses { get; set; } = new();
    }
}