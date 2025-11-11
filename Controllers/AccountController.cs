using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Dtos.Account;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<AppUser> _sign;
        private readonly ITokenService _token;

        public AccountController
        (UserManager<AppUser> userManager,
         ILogger<AccountController> logger,
         SignInManager<AppUser> sign,
         ITokenService token

        )
        {
            _userManager = userManager;
            _logger = logger;
            _sign = sign;
            _token = token;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // System.Console.WriteLine("We Are Here");
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Password Required");

            var appUser = new AppUser
            {
                UserName = dto.Username.Trim(),
                Email = dto.Email.Trim()
            };

            try
            {
                var user = await _userManager.CreateAsync(appUser, dto.Password);
                if (!user.Succeeded)
                    return BadRequest(new { errors = user.Errors });

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (!roleResult.Succeeded)
                    return BadRequest(new { errors = roleResult.Errors });

                var newUser = new NewUserDto
                {
                    Username = appUser.UserName,
                    Email = appUser.Email,
                    Token = _token.CreateToken(appUser)
                };
                return Ok(newUser);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ocured dure registration");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var userExist = await _userManager.Users
            .FirstOrDefaultAsync(u => u.UserName == dto.Username);

            if (userExist == null)
                return BadRequest("Invalid Username");

            var res = await _sign.CheckPasswordSignInAsync(userExist, dto.Password, false);
            if (!res.Succeeded)
                return Unauthorized("Incorrect Credntials");

            var user = new NewUserDto
            {
                Username = userExist.UserName!,
                Email = userExist.Email!,
                Token = _token.CreateToken(userExist)
            };

            return Ok(user);
        }

        [Authorize]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // Get the currently authenticated user
            var user = await _userManager.GetUserAsync(User);
            // Console.WriteLine(user + "User");
            if (user == null)
                return Unauthorized("User Not Found");

            if (!string.IsNullOrWhiteSpace(dto.Username))
                user.UserName = dto.Username.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email.Trim();

            var updateRes = await _userManager.UpdateAsync(user);
            if (!updateRes.Succeeded)
                return BadRequest(new { errors = updateRes.Errors });

            // Handle Password Changing if requested

            if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(dto.CurrentPassword))
                    return BadRequest("Current Password is Required to Change Password");

                var passwordRes = await _userManager
                .ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if (!passwordRes.Succeeded)
                    return BadRequest(new { errors = passwordRes.Errors });
            }
            var updatedUser = new NewUserDto
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = _token.CreateToken(user)
            };
            return Ok(updatedUser);
        }
        /// /// /// Delete Profile // // // // 
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteProfile()
        {
            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found!");

            var deletedUser = await _userManager.DeleteAsync(userExist);
            if (!deletedUser.Succeeded)
                return BadRequest(new { errors = deletedUser.Errors });

            return Ok("Deleted Successfully!");
        }
    }
}