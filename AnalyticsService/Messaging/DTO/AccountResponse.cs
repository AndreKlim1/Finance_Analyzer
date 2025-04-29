namespace AnalyticsService.Messaging.DTO
{
    public record AccountResponse(
        long Id,
        long UserId,
        string AccountName,
        string AccountType,
        string Currency,
        decimal Balance,
        int TransactionsCounts,
        string Description,
        string Color);
}
