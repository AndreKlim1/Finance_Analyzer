
namespace CaregoryAccountService.Models.DTO.Responses
{
    public record AccountResponse(
        long Id,
        long UserId,
        string AccountName,
        string AccountType,
        string Currency,
        int Balance,
        int TransactionsCounts,
        string Description,
        string Color);
}
