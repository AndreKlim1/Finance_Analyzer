using UsersService.Services.Interfaces;

namespace UsersService.Services
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
        IProfileService ProfileService { get; }
    }
}
