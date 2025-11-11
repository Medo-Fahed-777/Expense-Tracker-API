using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Dtos.Category;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Mappers;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _catRepo;
        private readonly UserManager<AppUser> _userManger;

        private readonly ILogger<CategoryController> _logger;


        public CategoryController
        (
            ICategoryRepo catRepo,
            ILogger<CategoryController> logger,
            UserManager<AppUser> userManager

        )
        {
            _catRepo = catRepo;
            _logger = logger;
            _userManger = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var userExist = await _userManger.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized();

            var cat = dto.CreateDto();

            cat.UserId = userExist.Id;

            var createdCat = await _catRepo.CreateAsync(cat);

            return CreatedAtAction(nameof(GetById), new { id = createdCat.Id }, createdCat.ToDto());
        }

        // // // Get Categories By User // // // 
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllByUser()
        {
            var userExist = await _userManger.GetUserAsync(User);

            if (userExist == null)
                return Unauthorized("User Not Found");

            var catsUser = await _catRepo.GetAllByUserIdAsync(userExist.Id);

            if (catsUser == null || !catsUser.Any())
                return NotFound("There is no Categories For this User");

            var cats = catsUser.Select(c => c.ToDto());

            return Ok(cats);
        }

        // // // Get Category By Id // // // 
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await _catRepo.GetByIdAsync(id);
            if (cat == null)
                return NotFound("Category Does not Exist");
            return Ok(cat.ToDto());
        }

        // // // Update Category // // // 
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var userExist = await _userManger.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");

            var cat = await _catRepo.UpdateAsync(id, dto.UpdateDto());
            if (cat == null)
                return BadRequest("Category Does not Exist");

            return Ok(cat.ToDto());
        }

        // // // Remove Category // // // 
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userExist = await _userManger.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");

            var catExist = await _catRepo.CategoryExist(id);
            if (catExist == null)
                return NotFound("Category Does not Exist");

            var hasExpenses = await _catRepo.HasExpenses(id);
            if (hasExpenses)
                return BadRequest("Cannot Delete a Category with existing Expenses");

            var cat = await _catRepo.DeleteAsync(id);
            if (cat == null)
                return NotFound("Category Does not Exist");
            
            return NoContent();
        }
    }
}