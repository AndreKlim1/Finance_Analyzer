using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Models.DTO.Requests
{
    public record UpdateAccountRequest(
        long Id,
        long UserId,
        string AccountName,
        string AccountType,
        string Currency,
        int Balance,
        string Description);
}
