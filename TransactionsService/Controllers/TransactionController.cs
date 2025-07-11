﻿using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using TransactionsService.Models.DTO.Responses;
using TransactionsService.Models.DTO.Requests;
using TransactionsService.Services.Interfaces;
using TransactionsService.Models.Errors;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace TransactionsService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/transactions")]
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResponse<TransactionResponse>>> GetTransactionsAsync( [FromQuery] TransactionFilterParameters filterParameters, CancellationToken token)
        {
            var result = await _transactionService.GetTransactionsAsync(filterParameters, token);

            return result.Match<ActionResult<PagedResponse<TransactionResponse>>>(
                onSuccess: () => Ok(new
                {
                    items = result.Value.Items,
                    totalPages = result.Value.TotalPages
                }),
                onFailure: error => NotFound(error));
        }

        [HttpGet("analytics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TransactionResponse>>> GetTransactionsAnalyticsAsync([FromQuery] TransactionFilterParameters filterParameters, CancellationToken token)
        {
            var result = await _transactionService.GetTransactionsAnalyticsAsync(filterParameters, token);

            return result.Match<ActionResult<List<TransactionResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTransactionAsync(CreateTransactionRequest createTransactionRequest, CancellationToken token)
        {
            var result = await _transactionService.CreateTransactionAsync(createTransactionRequest, token);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }

        [HttpPost("{fromAcc:long}/{toAcc:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TransferAsync(CreateTransactionRequest txData, long fromAcc, long toAcc, CancellationToken token)
        {
            var result = await _transactionService.CreateTransactionAsync(txData, token);

            if (result.IsSuccess) 
            {
                var toRequest = new CreateTransactionRequest(-txData.Value, txData.Title, txData.Currency, txData.CategoryId,
                                                             toAcc, txData.UserId, txData.Description, txData.Image, txData.TransactionDate,
                                                             txData.CreationDate, txData.TransactionType, txData.Merchant);

                result = await _transactionService.CreateTransactionAsync(toRequest, token);

                if (result.IsSuccess)
                    return Ok(result.Value);

                return BadRequest(result.Error);
            }
            else 
            {
                return BadRequest(result.Error);
            }
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
