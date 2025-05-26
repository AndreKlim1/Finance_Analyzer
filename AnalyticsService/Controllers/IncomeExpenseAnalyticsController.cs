using AnalyticsService.Models.DTO.Response.IncomeExpenseReport;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AnalyticsService.Models.Errors;
using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/analytics/income-expense")]
    public class IncomeExpenseAnalyticsController : ControllerBase
    {
        private readonly IIncomeExpenseAnalyticsService _analyticsService;

        public IncomeExpenseAnalyticsController(IIncomeExpenseAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("kpi")]
        [ProducesResponseType(typeof(ComparisonResponse<IncomeExpenseKpiDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ComparisonResponse<IncomeExpenseKpiDto>>> GetKpiAsync(
            [FromQuery] DateTime primaryStartDate,
            [FromQuery] DateTime primaryEndDate,
            [FromQuery] DateTime compareStartDate,
            [FromQuery] DateTime compareEndDate,
            [FromQuery] long userId,
            [FromQuery] string currency,
            CancellationToken token,
            [FromQuery] string? groupBy = null,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (primaryStartDate > primaryEndDate || compareStartDate > compareEndDate)
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты конца."));

            var result = await _analyticsService.GetKpiAsync(
                primaryStartDate, primaryEndDate,
                compareStartDate, compareEndDate,
                groupBy, accountIds, categoryIds, token, userId, currency);

            return result.Match<ActionResult<ComparisonResponse<IncomeExpenseKpiDto>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpGet("trend")]
        [ProducesResponseType(typeof(ComparisonResponse<IncomeExpenseTrendResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ComparisonResponse<IncomeExpenseTrendResponse>>> GetTrendAsync(
            [FromQuery] DateTime primaryStartDate,
            [FromQuery] DateTime primaryEndDate,
            [FromQuery] DateTime compareStartDate,
            [FromQuery] DateTime compareEndDate,
            [FromQuery] long userId,
            [FromQuery] string currency,
            [FromQuery] string groupBy,
            CancellationToken token,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (primaryStartDate > primaryEndDate || compareStartDate > compareEndDate)
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты конца."));
            if (string.IsNullOrWhiteSpace(groupBy))
                return BadRequest(new Error("Analytics.Validation", "Параметр 'groupBy' обязателен для тренда."));

            var result = await _analyticsService.GetTrendAsync(
                primaryStartDate, primaryEndDate,
                compareStartDate, compareEndDate,
                groupBy, accountIds, categoryIds, token, userId, currency);

            return result.Match<ActionResult<ComparisonResponse<IncomeExpenseTrendResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpGet("breakdown")]
        [ProducesResponseType(typeof(ComparisonResponse<IncomeExpenseBreakdownResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ComparisonResponse<IncomeExpenseBreakdownResponse>>> GetBreakdownAsync(
            [FromQuery] DateTime primaryStartDate,
            [FromQuery] DateTime primaryEndDate,
            [FromQuery] DateTime compareStartDate,
            [FromQuery] DateTime compareEndDate,
            [FromQuery] long userId,
            [FromQuery] string currency,
            CancellationToken token,
            [FromQuery] string? groupBy = null,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (primaryStartDate > primaryEndDate || compareStartDate > compareEndDate)
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты конца."));

            var result = await _analyticsService.GetBreakdownAsync(
                primaryStartDate, primaryEndDate,
                compareStartDate, compareEndDate,
                groupBy, accountIds, categoryIds, token, userId, currency);

            return result.Match<ActionResult<ComparisonResponse<IncomeExpenseBreakdownResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpGet("time-table")]
        [ProducesResponseType(typeof(ComparisonResponse<IEnumerable<TimeTableRowResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ComparisonResponse<IEnumerable<TimeTableRowResponse>>>> GetTimeTableAsync(
            [FromQuery] DateTime primaryStartDate,
            [FromQuery] DateTime primaryEndDate,
            [FromQuery] DateTime compareStartDate,
            [FromQuery] DateTime compareEndDate,
            [FromQuery] string groupBy,
            [FromQuery] long userId,
            [FromQuery] string currency,
            CancellationToken token,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (primaryStartDate > primaryEndDate || compareStartDate > compareEndDate)
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты конца."));
            if (string.IsNullOrWhiteSpace(groupBy))
                return BadRequest(new Error("Analytics.Validation", "Параметр 'groupBy' обязателен для таблицы времени."));

            var result = await _analyticsService.GetTimeTableAsync(
                primaryStartDate, primaryEndDate,
                compareStartDate, compareEndDate,
                groupBy, accountIds, categoryIds, token, userId, currency);

            return result.Match<ActionResult<ComparisonResponse<IEnumerable<TimeTableRowResponse>>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpGet("source-table")]
        [ProducesResponseType(typeof(ComparisonResponse<IEnumerable<CategoryAmountDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ComparisonResponse<IEnumerable<CategoryAmountDto>>>> GetSourceTableAsync(
            [FromQuery] DateTime primaryStartDate,
            [FromQuery] DateTime primaryEndDate,
            [FromQuery] DateTime compareStartDate,
            [FromQuery] DateTime compareEndDate,
            [FromQuery] long userId,
            [FromQuery] string currency,
            CancellationToken token,
            [FromQuery] string? groupBy = null,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (primaryStartDate > primaryEndDate || compareStartDate > compareEndDate)
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты конца."));

            var result = await _analyticsService.GetSourceTableAsync(
                primaryStartDate, primaryEndDate,
                compareStartDate, compareEndDate,
                groupBy, accountIds, categoryIds, token, userId, currency);

            return result.Match<ActionResult<ComparisonResponse<IEnumerable<CategoryAmountDto>>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpGet("category-table")]
        [ProducesResponseType(typeof(ComparisonResponse<IEnumerable<CategoryAmountDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ComparisonResponse<IEnumerable<CategoryAmountDto>>>> GetCategoryTableAsync(
            [FromQuery] DateTime primaryStartDate,
            [FromQuery] DateTime primaryEndDate,
            [FromQuery] DateTime compareStartDate,
            [FromQuery] DateTime compareEndDate,
            [FromQuery] long userId,
            [FromQuery] string currency,
            CancellationToken token,
            [FromQuery] string? groupBy = null,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (primaryStartDate > primaryEndDate || compareStartDate > compareEndDate)
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты конца."));

            var result = await _analyticsService.GetCategoryTableAsync(
                primaryStartDate, primaryEndDate,
                compareStartDate, compareEndDate,
                groupBy, accountIds, categoryIds, token, userId, currency);

            return result.Match<ActionResult<ComparisonResponse<IEnumerable<CategoryAmountDto>>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }
    }
}
