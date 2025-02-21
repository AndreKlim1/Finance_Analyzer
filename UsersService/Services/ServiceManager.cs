using UsersService.Repositories;
using UsersService.Services.Implementations;
using UsersService.Services.Interfaces;

namespace UsersService.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly IUserService _userService;
        private readonly IProfileService _profileService;

        public ServiceManager(IUserService userService, IProfileService profileService)
        {
            _userService = userService;
            _profileService = profileService;
        }

    }
}
