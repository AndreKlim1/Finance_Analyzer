using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;
using UsersService.Models.DTO.Requests;
using UsersService.Services.Implementations;
using UsersService.Services.Interfaces;

namespace UsersService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/auth")]
    public class AuthController(IAuthService authService, IConfiguration config) : ControllerBase
    {
        private readonly IConfiguration _config = config;

        [HttpPost("login")]       
        [ProducesResponseType(400), ProducesResponseType(200)]
        public async Task<ActionResult<string>> LoginAsync([FromBody] LoginUserRequest userRequest, CancellationToken ct)
        {
            var loginResult = await authService.LoginAsync(userRequest.Email, userRequest.PasswordHash, ct);
            if (loginResult == null) 
                return BadRequest();
            SetJwtCookie(loginResult);
            return Ok(loginResult);
        }

        [HttpPost("register")]
        [ProducesResponseType(400), ProducesResponseType(200)]
        public async Task<ActionResult<string>> RegisterAsync([FromBody] RegisterUserRequest userRequest, CancellationToken ct)
        {
            var profileCreate = new CreateProfileRequest(userRequest.FirstName, userRequest.LastName);
            var userCreate = new CreateUserRequest(userRequest.Role, userRequest.Email, userRequest.RegistrationDate, 1, userRequest.PasswordHash);
            var registerResult = await authService.RegisterAsync(userCreate, profileCreate, ct);
            if (registerResult == null) return BadRequest();
            SetJwtCookie(registerResult);
            return Ok(registerResult);
        }

        [HttpGet("verify")]
        [ProducesResponseType(200)]
        public IActionResult Verify()
        {
            if (!Request.Cookies.TryGetValue("token", out var token))
                return Unauthorized("Token not found");

            var principal = TokenValidator.ValidateToken(token, _config);
            if (principal == null) return Unauthorized("Invalid token");

            return Ok(new { message = "Token is valid" });
        }

        [HttpGet("userId")]
        [ProducesResponseType(200)]
        public IActionResult GetUserId()
        {
            if (!Request.Cookies.TryGetValue("token", out var token))
                return Unauthorized("Token not found");

            var principal = TokenValidator.ValidateToken(token, _config);
            if (principal == null) return Unauthorized("Invalid token");

            var userIdClaim = principal.FindFirst("UserId");
            if (userIdClaim == null) return Unauthorized("UserId not found in token");

            return Ok(new { userId = userIdClaim.Value });
        }


        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = false
            });
            return Ok(new { message = "Log out succesfully" });
        }

        private void SetJwtCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1),
                SameSite = SameSiteMode.None,
                Secure = false 
            };

            Response.Cookies.Append("token", token, cookieOptions);
        }
    }
}
