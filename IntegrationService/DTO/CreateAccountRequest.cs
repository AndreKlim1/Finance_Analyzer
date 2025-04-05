
namespace IntegrationService.DTO
{
    public record CreateAccountRequest(
        long UserId,
        string AccountName,
        string AccountType,
        string Currency,
        int Balance,
        int TransactionsCount,
        string Description,
        string Color);
}
