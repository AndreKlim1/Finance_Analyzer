using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using BudgetingService.Models.DTO.Responses;
using BudgetingService.Models.DTO.Requests;
using BudgetingService.Services.Interfaces;


namespace BudgetingService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _transactionService;

        public BudgetController(IBudgetService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BudgetResponse>> GetTransactionByIdAsync(long id, CancellationToken token)
        {
            var result = await _transactionService.GetTransactionByIdAsync(id, token);

            return result.Match<ActionResult<BudgetResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BudgetResponse>> GetTransactionByEmailAsync(string email, CancellationToken token)
        {
            var result = await _transactionService.GetTransactionByEmailAsync(email, token);

            return result.Match<ActionResult<BudgetResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTransactionAsync(CreateBudgetRequest createTransactionRequest, CancellationToken token)
        {
            var result = await _transactionService.CreateTransactionAsync(createTransactionRequest, token);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetTransactionByIdAsync), new { id = result.Value.Id }, result.Value);

            return BadRequest(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BudgetResponse>> UpdateTransactionAsync(UpdateBudgetRequest updateTransactionRequest,
            CancellationToken token)
        {
            var result = await _transactionService.UpdateTransactionAsync(updateTransactionRequest, token);

            return result.Match<ActionResult<BudgetResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTransactionAsync(long id, CancellationToken token)
        {
            var isDeleted = await _transactionService.DeleteTransactionAsync(id, token);

            return isDeleted ? NoContent() : BadRequest();
        }
    }
}
