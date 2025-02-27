using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;
using UsersService.Models.Errors;
using UsersService.Services.Implementations;
using UsersService.Services.Interfaces;

namespace UsersService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/profiles")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProfileResponse>> GetProfileByIdAsync(long id, CancellationToken token)
        {
            var result = await _profileService.GetProfileByIdAsync(id, token);

            return result.Match<ActionResult<ProfileResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error)); 
                
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProfileResponse>>> GetProfilesAsync(CancellationToken token)
        {
            var result = await _profileService.GetProfilesAsync(token);

            return result.Match<ActionResult<List<ProfileResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProfileAsync(CreateProfileRequest createProfileRequest, CancellationToken token)
        {
            var result = await _profileService.CreateProfileAsync(createProfileRequest, token);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProfileResponse>> UpdateProfileAsync(UpdateProfileRequest updateProfileRequest,
            CancellationToken token)
        {
            var result = await _profileService.UpdateProfileAsync(updateProfileRequest, token);

            return result.Match<ActionResult<ProfileResponse>>(
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
