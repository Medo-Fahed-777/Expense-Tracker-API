using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IReportService _reportService;
        private readonly UserManager<AppUser> _userManager;

        public ReportController
        (ILogger<ReportController> logger,
        IReportService reportService,
        UserManager<AppUser> userManager
        )
        {
            _logger = logger;
            _reportService = reportService;
            _userManager = userManager;
        }

        [HttpGet("MonthlyReport")]
        public async Task<IActionResult> GetMonthlyReport(int month, int year)
        {

            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12");

            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");

            var report = await _reportService.GenerateMonthlyReport(userExist.Id, month, year);
            if (report.TotalExpenses == 0)
                return NotFound("No Report Found for the Specified Month and Year");

            // Console.WriteLine(report);

            return Ok(report);

        }
        [HttpGet("SummaryReport")]
        public async Task<IActionResult> GetSummaryReport()
        {
            var userExist = await _userManager.GetUserAsync(User);
            if (userExist == null)
                return Unauthorized("User Not Found");

            var report = await _reportService.GenerateSummaryReport(userExist.Id);
            if (report.TotalExpensesRecorded == 0)
                return NotFound("No Summary Report Found for the User");

            return Ok(report);
        }
    }
}