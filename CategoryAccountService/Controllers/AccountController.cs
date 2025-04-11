using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using CaregoryAccountService.Services.Interfaces;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Services.Implementations;
using CaregoryAccountService.Models.Errors;
using CaregoryAccountService.Models.DTO.Requests;
using CategoryAccountService.Models.DTO;

namespace CaregoryAccountService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountResponse>> GetAccountByIdAsync(long id, CancellationToken token)
        {
            var result = await _accountService.GetAccountByIdAsync(id, token);

            return result.Match<ActionResult<AccountResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AccountResponse>>> GetAccountsAsync( CancellationToken token)
        {
            var result = await _accountService.GetAccountsAsync(token);

            return result.Match<ActionResult<List<AccountResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpGet("user/{userId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AccountResponse>>> GetAccountsByUserIdAsync(long userId, CancellationToken token)
        {
            var result = await _accountService.GetAccountsByUserIdAsync(userId, token);

            return result.Match<ActionResult<List<AccountResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAccountAsync(CreateAccountRequest createAccountRequest, CancellationToken token)
        {
            var result = await _accountService.CreateAccountAsync(createAccountRequest, token);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountResponse>> UpdateAccountAsync(UpdateAccountRequest updateAccountRequest,
            CancellationToken token)
        {
            var result = await _accountService.UpdateAccountAsync(updateAccountRequest, token);

            return result.Match<ActionResult<AccountResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAccountAsync(long id, CancellationToken token)
        {
            var isDeleted = await _accountService.DeleteAccountAsync(id, token);

            return isDeleted ? NoContent() : BadRequest();
        }

        /*[HttpPut("transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TransferBetweenAccountsAsync([FromBody] TransferRequest transferRequest, CancellationToken token)
        {
            var result = await _accountService.TransferBetweenAccountsAsync(transferRequest, token);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error)
            );
        }

        [HttpGet("{id:long}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetAccountBalanceAsync(long id, CancellationToken token)
        {
            var result = await _accountService.GetBalanceAsync(id, token);

            return result.Match<ActionResult<int>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error)
            );
        }

        [HttpPut("{id:long}/reconcile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReconcileAsync(long id, CancellationToken token)
        {
            var result = await _accountService.ReconcileAsync(id, token);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error)
            );
        }

        [HttpGet("{id:long}/statement")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetStatementAsync(long id, [FromQuery] DateRange dateRange, CancellationToken token)
        {
            var result = await _accountService.GetStatementAsync(id, dateRange, token);

            return result.Match<ActionResult<int>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error)
            );
        }
        */

    }
}
