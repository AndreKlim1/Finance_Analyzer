using Microsoft.AspNetCore.Mvc;
using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;
using UsersService.Services.Interfaces;
using Asp.Versioning;
using UsersService.Models.Errors;

namespace UsersService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetUserByIdAsync(long id, CancellationToken token)
        {
            var result = await _userService.GetUserByIdAsync(id, token);

            return result.Match<ActionResult<UserResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetUserByEmailAsync(string email, CancellationToken token)
        {
            var result = await _userService.GetUserByEmailAsync(email, token);

            return result.Match<ActionResult<UserResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest createUserRequest, CancellationToken token)
        {
            var result = await _userService.CreateUserAsync(createUserRequest, token);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetUserByIdAsync), new { id = result.Value.Id }, result.Value);

            return BadRequest(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> UpdateUserAsync(UpdateUserRequest updateUserRequest,
            CancellationToken token)
        {
            var result = await _userService.UpdateUserAsync(updateUserRequest, token);

            return result.Match<ActionResult<UserResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserAsync(long id, CancellationToken token)
        {
            var isDeleted = await _userService.DeleteUserAsync(id, token);

            return isDeleted ? NoContent() : BadRequest();
        }
    }
}
