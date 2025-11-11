using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MinLength(6 , ErrorMessage ="Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}