using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersService.Models.DTO.Requests;
using UsersService.Services.Interfaces;

namespace UsersService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(400), ProducesResponseType(200)]
        public async Task<ActionResult<string>> LoginAsync([FromBody] LoginUserRequest userRequest, CancellationToken ct)
        {
            var loginResult = await authService.LoginAsync(userRequest.Email, userRequest.PasswordHash, ct);
            if (loginResult == null) return BadRequest();
            return Ok(loginResult);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(400), ProducesResponseType(200)]
        public async Task<ActionResult<string>> RegisterAsync([FromBody] CreateUserRequest userRequest, CancellationToken ct)
        {
            var registerResult = await authService.RegisterAsync(userRequest, ct);
            if (registerResult == null) return BadRequest();
            return Ok(registerResult);
        }
    }
}
