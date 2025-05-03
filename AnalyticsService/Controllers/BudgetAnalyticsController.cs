using AnalyticsService.Models.DTO.Response.BudgetAnalytics;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AnalyticsService.Models.Errors;
using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/analytics/budget")]
    public class BudgetAnalyticsController : ControllerBase
    {
        private readonly IBudgetAnalyticsService _analyticsService;

        public BudgetAnalyticsController(IBudgetAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("{budgetId:long}/budget-trend")]
        [ProducesResponseType(typeof(BudgetTrendResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BudgetTrendResponse>> GetBudgetTrendAsync(
            long budgetId,
            [FromQuery] long userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            CancellationToken token)
        {
            if (userId == null)
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'userId' не может быть пустыми."));
            }
            if (budgetId <= 0)
            {
                return BadRequest(new Error("Analytics.Validation", "Некорректный ID бюджета."));
            }
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }

            var result = await _analyticsService.GetBudgetTrendAsync(userId, budgetId, startDate, endDate, token);

            return result.Match<ActionResult<BudgetTrendResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("Validation")
                                    ? BadRequest(error)
                                    : NotFound(error));
        }

        /// <param name="period">Период ('thisMonth', 'last30days', 'thisYear' и т.д.).</param>
        /// <param name="type">Тип транзакций ('income' или 'expense').</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("{budgetId:long}/category-breakdown")]
        [ProducesResponseType(typeof(BudgetCategoryBreakdownResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BudgetCategoryBreakdownResponse>> GetBudgetCategoryBreakdownAsync(
            long budgetId,
            [FromQuery] long userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            CancellationToken token)
        {
            if (budgetId <= 0)
            {
                return BadRequest(new Error("Analytics.Validation", "Некорректный ID бюджета."));
            }
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }

            var result = await _analyticsService.GetBudgetCategoryBreakdownAsync(userId, budgetId, startDate, endDate, token);

            return result.Match<ActionResult<BudgetCategoryBreakdownResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("Validation")
                                    ? BadRequest(error)
                                    : NotFound(error));
        }
    }
}
