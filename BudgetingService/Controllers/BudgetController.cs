using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using BudgetingService.Models.DTO.Responses;
using BudgetingService.Models.DTO.Requests;
using BudgetingService.Services.Interfaces;
using BudgetingService.Models.Errors;
using BudgetingService.Services.Implementations;


namespace BudgetingService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/budgets")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BudgetResponse>> GetBudgetByIdAsync(long id, CancellationToken token)
        {
            var result = await _budgetService.GetBudgetByIdAsync(id, token);

            return result.Match<ActionResult<BudgetResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BudgetResponse>>> GetBudgetsAsync(CancellationToken token)
        {
            var result = await _budgetService.GetBudgetsAsync(token);

            return result.Match<ActionResult<List<BudgetResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBudgetAsync(CreateBudgetRequest createBudgetRequest, CancellationToken token)
        {
            var result = await _budgetService.CreateBudgetAsync(createBudgetRequest, token);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BudgetResponse>> UpdateBudgetAsync(UpdateBudgetRequest updateBudgetRequest,
            CancellationToken token)
        {
            var result = await _budgetService.UpdateBudgetAsync(updateBudgetRequest, token);

            return result.Match<ActionResult<BudgetResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBudgetAsync(long id, CancellationToken token)
        {
            var isDeleted = await _budgetService.DeleteBudgetAsync(id, token);

            return isDeleted ? NoContent() : BadRequest();
        }
    }
}
