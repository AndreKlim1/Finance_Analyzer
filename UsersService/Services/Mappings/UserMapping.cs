using System.Runtime.CompilerServices;
using UsersService.Models;
using UsersService.Models.DTO.Requests;
using UsersService.Models.DTO.Responses;
using UsersService.Models.Enums;

namespace UsersService.Services.Mappings
{
    public static class UserMapping
    {
        public static UserResponse ToUserResponse(this User user)
        {
            return new UserResponse
            ( 
                user.Id,
                user.Role.ToString(),
                user.Email,
                user.RegistrationDate,
                user.ProfileId
            );
        }

        public static User ToUser(this CreateUserRequest request)
        {
            return new User
            {
                Role = Enum.Parse<Role>(request.Role),
                Email = request.Email,
                RegistrationDate = request.RegistrationDate,
                ProfileId = request.ProfileId,
            };
        }

        public static User ToUser(this UpdateUserRequest request)
        {
            return new User
            {
                Id = request.Id,
                Role = Enum.Parse<Role>(request.Role),
                Email = request.Email,
                RegistrationDate = request.RegistrationDate,
                ProfileId = request.ProfileId,
            };
        }
    }
}
