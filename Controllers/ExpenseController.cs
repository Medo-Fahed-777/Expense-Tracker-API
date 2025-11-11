using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Dtos.Expence;
using ExpenseTracker.Helpers;
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
    public class ExpenseController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ExpenseController> _logger;
        private readonly IExpenseRepo _repo;
        private readonly ICategoryRepo _catRepo;

        public ExpenseController
        (
            UserManager<AppUser> userManager,
            ILogger<ExpenseController> logger,
            IExpenseRepo repo,
            ICategoryRepo catRepo
        )
        {
            _userManager = userManager;
            _logger = logger;
            _repo = repo;
            _catRepo = catRepo;
        }
        [HttpPost("{categoryId:int}")]
        public async Task<IActionResult> Create(int categoryId, [FromBody] CreateExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");

            var catExist = await _catRepo.CategoryExist(categoryId);
            if (catExist == null)
                return NotFound("Category Does not Exist");

            var expense = dto.CreateDto();
            expense.UserId = userExist.Id;
            expense.CategoryId = catExist.Id;
            expense.Category = catExist;
            expense.User = userExist;

            await _repo.CeeateAsync(categoryId, expense);
            return CreatedAtAction(nameof(GetById), new {id = expense.Id } , expense.ToDto());
        }
        // // // Geet All // // // 
        [HttpGet]
        public async Task<IActionResult> GetAll( [FromQuery] QueryObject query )
        {
            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");
            
            if(query.MonthNumber < 0 || query.MonthNumber > 12)
                return BadRequest("Month Number must be between 1 and 12");
            

            var expenses = await _repo.GetAllAsync(userExist.Id, query);
            if (expenses == null)
                return NotFound("There is no Expenses for this user");

            return Ok(expenses.Select(e => e.ToDto()));
        }

        // // // Get By Id // // // 
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _repo.GetByIdAsync(id);
            if (expense == null)
                return NotFound("Expense Does not Exist");

            return Ok(expense.ToDto());
        }

        // // // Update Expense // // // 
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");

            var expense = await _repo.UpdateAsync(id, dto.UpdateDto());
            if (expense == null)
                return NotFound("Expense Does not Exist");

            return Ok(expense.ToDto());
        }

        // // // Get Expenses By Category Id // // // 
        [HttpGet("[action]/{categoryId:int}")]
        public async Task<IActionResult> GetByCategoryId([FromRoute] int categoryId)
        {
            var catExist = await _catRepo.CategoryExist(categoryId);
            if (catExist == null)
                return NotFound("Category Does not Exist");

            var expenses = await _repo.GetByCategoryId(categoryId);
            if (expenses == null)
                return NotFound("There is no Expenses For this Category");

            return Ok(expenses.Select(e => e.ToDto()));
        }

        // // // Delete Expense // // // 
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");

            var expenseToDelete = await _repo.DeleteAsync(id);
            if (expenseToDelete == null)
                return NotFound("Expense Does not Exist");

            return NoContent();
        }

        // // // Get a Miimum Amount // // // 
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMinimumAmount()
        {
            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("Usr Not Found");

            var minExpense = await _repo.ReturnMinAmount(userExist.Id);
            if (minExpense == null)
                return NotFound("There is NO Amount");


            return Ok(minExpense.ToDto());
        }

        // // // Get Maximum Amount // // // 
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMaximumAmount()
        {
            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");

            var maxExpense = await _repo.ReturnMaxAmount(userExist.Id);
            if (maxExpense == null)
                return NotFound("There is no Expense");

            return Ok(maxExpense.ToDto());
        }

        // // // Get Expenses By Month Name // // // 
        [HttpGet("[action]/{monthNumber:int}/{year:int}")]
        public async Task<IActionResult> GetByMonth([FromRoute] int monthNumber, [FromRoute] int year)
        {
            if (monthNumber < 1 || monthNumber > 12)
                return BadRequest("Month Number must be between 1 and 12");

            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");

            var expenses = await _repo.GetByMonth(userExist.Id, monthNumber, year);
            if (expenses == null || expenses.Count == 0)
                return NotFound("There is no Expenses for this Month");

            return Ok(expenses.Select(e => e.ToDto()));
        }

        // // // Get Current Monthe Expenses // // // 
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCurrentMonthExpenses()
        {
            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");

            var expenses = await _repo.GetCurrentMonthExpenses(userExist.Id);
            if (expenses == null || expenses.Count == 0)
                return NotFound("There is no Expenses for this Month");

            return Ok(expenses.Select(e => e.ToDto()));
        }

    }
}