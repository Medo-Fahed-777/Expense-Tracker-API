using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Dtos.Account
{
    public class UpdateUserDto
    {
        public string Username { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [MinLength(6 , ErrorMessage ="Password must be at least 6 characters")]
        public string CurrentPassword { get; set; } = string.Empty;
        [MinLength(6 , ErrorMessage ="Password must be at least 6 characters")]
        public string NewPassword { get; set; } = string.Empty;
    }
}