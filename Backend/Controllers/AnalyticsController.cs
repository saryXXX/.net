using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var result = await _analyticsService.GetGeneralStatsAsync();
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("turnover/{year}")]
        public async Task<IActionResult> GetTurnoverByYear(int year)
        {
            var result = await _analyticsService.GetTurnoverByMonthAsync(year);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("fiscal")]
        public async Task<IActionResult> GetFiscalReport([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var result = await _analyticsService.GetFiscalSummaryAsync(start, end);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventoryReport()
        {
            var result = await _analyticsService.GetInventorySummaryAsync();
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
