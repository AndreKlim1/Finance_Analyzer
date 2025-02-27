using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;
using UsersService.Models.Errors;

namespace UsersService.Services.Interfaces
{
    public interface IProfileService
    {
        Task<Result<ProfileResponse>> GetProfileByIdAsync(long id, CancellationToken token);
        Task<Result<List<ProfileResponse>>> GetProfilesAsync( CancellationToken token);
        Task<Result<ProfileResponse>> CreateProfileAsync(CreateProfileRequest createProfileRequest, CancellationToken token);
        Task<Result<ProfileResponse>> UpdateProfileAsync(UpdateProfileRequest updateProfileRequest, CancellationToken token);
        Task<bool> DeleteProfileAsync(long id, CancellationToken token);
    }
}
