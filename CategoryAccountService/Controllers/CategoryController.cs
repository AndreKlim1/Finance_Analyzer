using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;
using UsersService.Models.Errors;
using UsersService.Services.Implementations;
using UsersService.Services.Interfaces;

namespace CaregoryAccountService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _profileService;

        public CategoryController(ICategoryService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> GetProfileByIdAsync(int id, CancellationToken token)
        {
            var result = await _profileService.GetProfileByIdAsync(id, token);

            return result.Match<ActionResult<CategoryResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error)); 
                
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProfileAsync(CreateCategoryRequest createProfileRequest, CancellationToken token)
        {
            var result = await _profileService.CreateProfileAsync(createProfileRequest, token);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetProfileByIdAsync), new { id = result.Value.Id }, result.Value);

            return BadRequest(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryResponse>> UpdateProfileAsync(UpdateCategoryRequest updateProfileRequest,
            CancellationToken token)
        {
            var result = await _profileService.UpdateProfileAsync(updateProfileRequest, token);

            return result.Match<ActionResult<CategoryResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProfileAsync(long id, CancellationToken token)
        {
            var isDeleted = await _profileService.DeleteProfileAsync(id, token);

            return isDeleted ? NoContent() : BadRequest();
        }
    }
}
