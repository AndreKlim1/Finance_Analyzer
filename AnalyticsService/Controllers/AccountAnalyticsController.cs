using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AnalyticsService.Models.Errors;
using AnalyticsService.Models.DTO.Response.AccountAnalytics;
using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")] 
    [Route("api/v1.0/analytics/account")] 
    public class AccountAnalyticsController : ControllerBase
    {
        private readonly IAccountAnalyticsService _analyticsService; 

        public AccountAnalyticsController(IAccountAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
 
        /// <param name="accountId">ID счета.</param>
        /// <param name="startDate">Дата начала периода (включая).</param>
        /// <param name="endDate">Дата конца периода (включая).</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("{accountId:long}/kpi")]
        [ProducesResponseType(typeof(AccountKpiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountKpiResponse>> GetAccountKpiAsync(
            long accountId,
            [FromQuery] long userId,
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

            var result = await _analyticsService.GetAccountKpiAsync(userId, accountId, startDate, endDate, token);

            return result.Match<ActionResult<AccountKpiResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("NotFound")
                                    ? NotFound(error)
                                    : BadRequest(error)); 
        }

        
        /// <param name="accountId">ID счета.</param>
        /// <param name="startDate">Дата начала периода (включая).</param>
        /// <param name="endDate">Дата конца периода (включая).</param>
        /// <param name="token">Токен отмены операции.</param>
        
        [HttpGet("{accountId:long}/balance-trend")]
        [ProducesResponseType(typeof(AccountBalanceTrendResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountBalanceTrendResponse>> GetAccountBalanceTrendAsync(
            long accountId,
            [FromQuery] long userId,
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

            
            var result = await _analyticsService.GetAccountBalanceTrendAsync(userId, accountId, startDate, endDate, token);

            return result.Match<ActionResult<AccountBalanceTrendResponse>>(
                 onSuccess: () => Ok(result.Value),
                 onFailure: error => error.Code.Contains("NotFound")
                                     ? NotFound(error)
                                     : BadRequest(error));
        }

        
        /// <param name="accountId">ID счета.</param>
        /// <param name="startDate">Дата начала периода (включая).</param>
        /// <param name="endDate">Дата конца периода (включая).</param>
        /// <param name="type">Тип транзакций ('income' или 'expense').</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("{accountId:long}/category-breakdown")]
        [ProducesResponseType(typeof(AccountCategoryBreakdownResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountCategoryBreakdownResponse>> GetAccountCategoryBreakdownAsync(
            long accountId,
            [FromQuery] long userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string type, 
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
            if (string.IsNullOrWhiteSpace(type) || (type.ToLowerInvariant() != "income" && type.ToLowerInvariant() != "expense"))
            {
                return BadRequest(new Error("Analytics.Validation", "Параметр 'type' должен быть 'income' или 'expense'."));
            }

            
            var result = await _analyticsService.GetAccountCategoryBreakdownAsync(userId, accountId, startDate, endDate, type, token);

            return result.Match<ActionResult<AccountCategoryBreakdownResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => error.Code.Contains("NotFound")
                                    ? NotFound(error)
                                    : BadRequest(error));
        }
    }
}
