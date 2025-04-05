using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Models.DTO.Requests
{
    public record UpdateAccountRequest(
        long Id,
        long UserId,
        string AccountName,
        string AccountType,
        string Currency,
        decimal Balance,
        int TransactionsCount,
        string Description,
        string Color);
}
