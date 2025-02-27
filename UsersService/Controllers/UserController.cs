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
    [Route("api/v1.0" +/*{version:apiVersion}*/"/users")]
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserResponse>>> GetUsersAsync(CancellationToken token)
        {
            var result = await _userService.GetUsersAsync(token);

            return result.Match<ActionResult<List<UserResponse>>>(
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest createUserRequest, CancellationToken token)
        {
            var result = await _userService.CreateUserAsync(createUserRequest, token);

            if (result.IsSuccess)
                return Ok(result.Value);

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
/*usersservice:
    image: ${DOCKER_REGISTRY-}usersservice
    container_name: usersservice
    build:
      context: .
      dockerfile: UsersService/Dockerfile
    environment:
      - ASPNETCORE_URLS=http://+:8080
    ports:
      - "24110:8080"
    depends_on:
      - postgres
  
  budgetingservice:
    image: ${DOCKER_REGISTRY-}budgetingservice
    container_name: budgetingservice
    build:
      context: .
      dockerfile: BudgetingService/Dockerfile
    environment:
      - ASPNETCORE_URLS=http://+:8080
    ports:
      - "24116:8080"
    depends_on:
      - postgres

  categoryaccountservice:
    image: ${DOCKER_REGISTRY-}categoryaccountservice
    container_name: categoryaccountservice
    build:
      context: .
      dockerfile: CategoryAccountService/Dockerfile
    environment:
      - ASPNETCORE_URLS=http://+:8080
    ports:
      - "24114:8080"
    depends_on:
      - postgres

  transactionsservice:
    image: ${DOCKER_REGISTRY-}transactionsservice
    container_name: transactionsservice
    build:
      context: .
      dockerfile: TransactionsService/Dockerfile
    environment:
      - ASPNETCORE_URLS=http://+:8080
    ports:
      - "24112:8080"
    depends_on:
      - postgres*/