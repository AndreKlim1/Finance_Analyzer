using CaregoryAccountService.Repositories;
using CaregoryAccountService.Services.Implementations;
using CaregoryAccountService.Services.Interfaces;

namespace CaregoryAccountService.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly IAccountService _userService;
        private readonly ICategoryService _profileService;

        public ServiceManager(IAccountService userService, ICategoryService profileService)
        {
            _userService = userService;
            _profileService = profileService;
        }

    }
}
