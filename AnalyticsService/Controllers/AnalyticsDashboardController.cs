using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AnalyticsService.Models.Errors;
using AnalyticsService.Models.DTO.Response;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/analytics/dashboard")]
    public class AnalyticsDashboardController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsDashboardController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        
        /// <param name="period">Период ('thisMonth', 'last30days', 'thisYear' и т.д.).</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("kpi")]
        [ProducesResponseType(typeof(KpiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<KpiResponse>> GetKpiAsync([FromQuery] string period, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(period))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'period' не может быть пустым."));
            }

            var result = await _analyticsService.GetKpiAsync(period, token);

            return result.Match<ActionResult<KpiResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("Validation") 
                                    ? BadRequest(error)
                                    : NotFound(error)); 
        }

        
        /// <param name="period">Период ('thisMonth', 'last30days', 'thisYear' и т.д.).</param>
        /// <param name="granularity">Гранулярность ('daily', 'weekly', 'monthly').</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("income-expense-trend")]
        [ProducesResponseType(typeof(IncomeExpenseTrendResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IncomeExpenseTrendResponse>> GetIncomeExpenseTrendAsync(
            [FromQuery] string period,
            [FromQuery] string granularity,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(period) || string.IsNullOrWhiteSpace(granularity))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметры 'period' и 'granularity' не могут быть пустыми."));
            }

            var result = await _analyticsService.GetIncomeExpenseTrendAsync(period, granularity, token);

            return result.Match<ActionResult<IncomeExpenseTrendResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("Validation")
                                    ? BadRequest(error)
                                    : NotFound(error));
        }

        /// <param name="period">Период ('thisMonth', 'last30days', 'thisYear' и т.д.).</param>
        /// <param name="type">Тип транзакций ('income' или 'expense').</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("category-breakdown")]
        [ProducesResponseType(typeof(CategoryBreakdownResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryBreakdownResponse>> GetCategoryBreakdownAsync(
            [FromQuery] string period,
            [FromQuery] string type, // Возможно, лучше использовать enum здесь
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(period) || string.IsNullOrWhiteSpace(type))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметры 'period' и 'type' не могут быть пустыми."));
            }
            if (type.ToLowerInvariant() != "income" && type.ToLowerInvariant() != "expense")
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'type' должен быть 'income' или 'expense'."));
            }

            var result = await _analyticsService.GetCategoryBreakdownAsync(period, type, token);

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
            [FromQuery] string period,
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(period))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'period' не может быть пустым."));
            }
            if (accountId <= 0)
            {
                return BadRequest(new Error("Analytics.Validation", "Некорректный ID счета."));
            }


            var result = await _analyticsService.GetAccountSparklineAsync(accountId, period, token);

            return result.Match<ActionResult<SparklineResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("Validation")
                                    ? BadRequest(error)
                                    : NotFound(error));
        }
    }
}
