using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AnalyticsService.Models.Errors;
using AnalyticsService.Models.DTO.Response.AnalyticsDashboard;
using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/analytics/dashboard")]
    public class AnalyticsDashboardController : ControllerBase
    {
        private readonly IAnalyticsDashboardService _analyticsService;

        public AnalyticsDashboardController(IAnalyticsDashboardService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        
        /// <param name="period">Период ('thisMonth', 'last30days', 'thisYear' и т.д.).</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("{accountId:long}/kpi")]
        [ProducesResponseType(typeof(KpiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<KpiResponse>> GetKpiAsync(long accountId, [FromQuery] long userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken token)
        {
            if (accountId <= 0)
            {
                return BadRequest(new Error("Analytics.Validation", "Некорректный ID счета."));
            }
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }

            var result = await _analyticsService.GetKpiAsync(userId, accountId, startDate, endDate, token);

            return result.Match<ActionResult<KpiResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("Validation") 
                                    ? BadRequest(error)
                                    : NotFound(error)); 
        }

        
        /// <param name="period">Период ('thisMonth', 'last30days', 'thisYear' и т.д.).</param>
        /// <param name="granularity">Гранулярность ('daily', 'weekly', 'monthly').</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("{accountId:long}/income-expense-trend")]
        [ProducesResponseType(typeof(IncomeExpenseTrendResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IncomeExpenseTrendResponse>> GetIncomeExpenseTrendAsync(
            long accountId,
            [FromQuery] long userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string granularity,
            CancellationToken token)
        {
            if (userId != null || string.IsNullOrWhiteSpace(granularity))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметры 'userId' и 'granularity' не могут быть пустыми."));
            }
            if (accountId <= 0)
            {
                return BadRequest(new Error("Analytics.Validation", "Некорректный ID счета."));
            }
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }

            var result = await _analyticsService.GetIncomeExpenseTrendAsync(userId, accountId, startDate, endDate, granularity, token);

            return result.Match<ActionResult<IncomeExpenseTrendResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("Validation")
                                    ? BadRequest(error)
                                    : NotFound(error));
        }

        /// <param name="period">Период ('thisMonth', 'last30days', 'thisYear' и т.д.).</param>
        /// <param name="type">Тип транзакций ('income' или 'expense').</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("{accountId:long}/category-breakdown")]
        [ProducesResponseType(typeof(CategoryBreakdownResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryBreakdownResponse>> GetCategoryBreakdownAsync(
            long accountId,
            [FromQuery] long userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string type, // Возможно, лучше использовать enum здесь
            CancellationToken token)
        {
            if (accountId <= 0)
            {
                return BadRequest(new Error("Analytics.Validation", "Некорректный ID счета."));
            }
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }
            if (type.ToLowerInvariant() != "income" && type.ToLowerInvariant() != "expense")
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'type' должен быть 'income' или 'expense'."));
            }

            var result = await _analyticsService.GetCategoryBreakdownAsync(userId, accountId, startDate, endDate, type, token);

            return result.Match<ActionResult<CategoryBreakdownResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("Validation")
                                    ? BadRequest(error)
                                    : NotFound(error));
        }

        /// <param name="accountId">ID счета.</param>
        /// <param name="period">Период ('last7days', 'last30days' и т.д.).</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("{accountId:long}/sparkline")] 
        [ProducesResponseType(typeof(SparklineResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)] 
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SparklineResponse>> GetAccountSparklineAsync(
            long accountId,
            [FromBody] long userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            CancellationToken token)
        {
            if (accountId <= 0)
            {
                return BadRequest(new Error("Analytics.Validation", "Некорректный ID счета."));
            }
            if (startDate > endDate)
            {
                return BadRequest(new Error("Analytics.Validation", "Дата начала не может быть позже даты окончания."));
            }


            var result = await _analyticsService.GetAccountSparklineAsync(accountId, userId, startDate, endDate, token);

            return result.Match<ActionResult<SparklineResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("Validation")
                                    ? BadRequest(error)
                                    : NotFound(error));
        }
    }
}
