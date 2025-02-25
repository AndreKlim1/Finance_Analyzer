using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using TransactionsService.Models.DTO.Responses;
using TransactionsService.Models.DTO.Requests;
using TransactionsService.Services.Interfaces;


namespace TransactionsService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionResponse>> GetTransactionByIdAsync(long id, CancellationToken token)
        {
            var result = await _transactionService.GetTransactionByIdAsync(id, token);

            return result.Match<ActionResult<TransactionResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionResponse>> GetTransactionByEmailAsync(string email, CancellationToken token)
        {
            var result = await _transactionService.GetTransactionByEmailAsync(email, token);

            return result.Match<ActionResult<TransactionResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTransactionAsync(CreateTransactionRequest createTransactionRequest, CancellationToken token)
        {
            var result = await _transactionService.CreateTransactionAsync(createTransactionRequest, token);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetTransactionByIdAsync), new { id = result.Value.Id }, result.Value);

            return BadRequest(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionResponse>> UpdateTransactionAsync(UpdateTransactionRequest updateTransactionRequest,
            CancellationToken token)
        {
            var result = await _transactionService.UpdateTransactionAsync(updateTransactionRequest, token);

            return result.Match<ActionResult<TransactionResponse>>(
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
