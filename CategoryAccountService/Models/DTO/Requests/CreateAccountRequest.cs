
namespace CaregoryAccountService.Models.DTO.Requests
{
    public record CreateAccountRequest(
        long UserId,
        string AccountName,
        string AccountType,
        string Currency,
        int Balance,
        string Description);
}
