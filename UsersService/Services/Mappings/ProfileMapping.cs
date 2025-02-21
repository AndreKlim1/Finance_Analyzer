using System.Runtime.CompilerServices;
using UsersService.Models;
using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;

namespace UsersService.Services.Mappings
{
    public static class ProfileMapping
    {
        public static ProfileResponse ToProfileResponse(this Profile profile)
        {
            return new ProfileResponse(
                profile.Id,
                profile.FirstName,
                profile.LastName);
        }

        public static Profile ToProfile(this CreateProfileRequest request)
        {
            return new Profile
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
        }

        public static Profile ToProfile(this UpdateProfileRequest request)
        {
            return new Profile
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
        }
    }
}
