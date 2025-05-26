using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AnalyticsService.Models.Errors;
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Models.DTO.Response.SpendingPatternsReport;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/analytics/spending-patterns")]
    public class SpendingPatternsReportController : ControllerBase
    {
        private readonly ISpendingPatternsReportService _analyticsService;

        public SpendingPatternsReportController(ISpendingPatternsReportService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("kpi")]
        [ProducesResponseType(typeof(SpendingPatternsKpiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SpendingPatternsKpiResponse>> GetSpendingPatternsKpiAsync(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string targetCurrency,
            [FromQuery] long userId,
            CancellationToken token,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }
            if (string.IsNullOrWhiteSpace(targetCurrency))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'targetCurrency' обязателен."));
            }

            var result = await _analyticsService.GetSpendingPatternsKpiAsync(startDate, endDate, targetCurrency, accountIds, categoryIds, token, userId);

            return result.Match<ActionResult<SpendingPatternsKpiResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpGet("day-of-week")]
        [ProducesResponseType(typeof(IEnumerable<DayOfWeekPatternResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DayOfWeekPatternResponse>>> GetDayOfWeekAsync(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string targetCurrency,
            [FromQuery] long userId,
            CancellationToken token,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }
            if (string.IsNullOrWhiteSpace(targetCurrency))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'targetCurrency' обязателен."));
            }

            var result = await _analyticsService.GetDayOfWeekPatternsAsync(startDate, endDate, targetCurrency, accountIds, categoryIds, token, userId);

            return result.Match<ActionResult<IEnumerable<DayOfWeekPatternResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpGet("time-of-day")]
        [ProducesResponseType(typeof(IEnumerable<TimeOfDayPatternResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TimeOfDayPatternResponse>>> GetTimeOfDayAsync(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string targetCurrency,
            [FromQuery] long userId,
            CancellationToken token,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }
            if (string.IsNullOrWhiteSpace(targetCurrency))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'targetCurrency' обязателен."));
            }

            var result = await _analyticsService.GetTimeOfDayPatternsAsync(startDate, endDate, targetCurrency, accountIds, categoryIds, token, userId);

            return result.Match<ActionResult<IEnumerable<TimeOfDayPatternResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpGet("value-distribution")]
        [ProducesResponseType(typeof(ValueDistributionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ValueDistributionResponse>> GetValueDistributionAsync(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string targetCurrency,
            [FromQuery] long userId,
            CancellationToken token,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }
            if (string.IsNullOrWhiteSpace(targetCurrency))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'targetCurrency' обязателен."));
            }

            var result = await _analyticsService.GetValueDistributionAsync(startDate, endDate, targetCurrency, accountIds, categoryIds, token, userId);

            return result.Match<ActionResult<ValueDistributionResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpGet("avg-check-trend")]
        [ProducesResponseType(typeof(AvgCheckTrendResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AvgCheckTrendResponse>> GetAvgCheckTrendAsync(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string targetCurrency,
            [FromQuery] long userId,
            CancellationToken token,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }
            if (string.IsNullOrWhiteSpace(targetCurrency))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'targetCurrency' обязателен."));
            }

            var result = await _analyticsService.GetAvgCheckTrendAsync(startDate, endDate, targetCurrency, accountIds, categoryIds, token, userId);

            return result.Match<ActionResult<AvgCheckTrendResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpGet("largest-transactions")]
        [ProducesResponseType(typeof(IEnumerable<LargestTransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LargestTransactionResponse>>> GetLargestTransactionsAsync(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string targetCurrency,
            [FromQuery] long userId,
            CancellationToken token,
            [FromQuery] string? accountIds = null,
            [FromQuery] string? categoryIds = null
            )
        {
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }
            if (string.IsNullOrWhiteSpace(targetCurrency))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'targetCurrency' обязателен."));
            }

            var result = await _analyticsService.GetLargestTransactionsAsync(startDate, endDate, targetCurrency, accountIds, categoryIds, token, userId);

            return result.Match<ActionResult<IEnumerable<LargestTransactionResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }
    }
}

