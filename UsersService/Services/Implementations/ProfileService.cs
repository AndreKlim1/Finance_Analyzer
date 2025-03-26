using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;
using UsersService.Models.Errors;
using UsersService.Repositories.Implementations;
using UsersService.Repositories.Interfaces;
using UsersService.Services.Interfaces;
using UsersService.Services.Mappings;
using UsersService.Services.Validators;

namespace UsersService.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
            
        }

        public async Task<Result<ProfileResponse>> CreateProfileAsync(CreateProfileRequest createProfileRequest, CancellationToken token)
        {
            var profile = createProfileRequest.ToProfile();

            await _profileRepository.AddAsync(profile, token);

            return Result<ProfileResponse>.Success(profile.ToProfileResponse());
        }

        public async Task<bool> DeleteProfileAsync(long id, CancellationToken token)
        {
            await _profileRepository.DeleteAsync(id, token);

            return true;
        }

        public async Task<Result<ProfileResponse>> GetProfileByIdAsync(long id, CancellationToken token)
        {
            var profile = await _profileRepository.GetByIdAsync(id, token);

            return profile is not null
                ? Result<ProfileResponse>.Success(profile.ToProfileResponse())
                : Result<ProfileResponse>.Failure(ProfileErrors.ProfileNotFound);
        }

        public async Task<Result<List<ProfileResponse>>> GetProfilesAsync(CancellationToken token)
        {
            var profiles = await _profileRepository.FindAll(true).ToListAsync();

            if (profiles is null)
            {
                return Result<List<ProfileResponse>>.Failure(ProfileErrors.ProfileNotFound);
            }
            else
            {
                var responses = new List<ProfileResponse>();
                foreach (var profile in profiles)
                {
                    responses.Add(profile.ToProfileResponse());
                }
                return Result<List<ProfileResponse>>.Success(responses);
            }
        }

        public async Task<Result<ProfileResponse>> UpdateProfileAsync(UpdateProfileRequest updateProfileRequest, CancellationToken token)
        {
            var profile = updateProfileRequest.ToProfile();
            profile = await _profileRepository.UpdateAsync(profile, token);

            return profile is null
                ? Result<ProfileResponse>.Failure(ProfileErrors.ProfileNotFound)
                : Result<ProfileResponse>.Success(profile.ToProfileResponse());
        }
    }
}
