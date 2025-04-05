
namespace CaregoryAccountService.Models.DTO.Requests
{
    public record CreateAccountRequest(
        long UserId,
        string AccountName,
        string AccountType,
        string Currency,
        decimal Balance,
        int TransactionsCount,
        string Description,
        string Color);
}
