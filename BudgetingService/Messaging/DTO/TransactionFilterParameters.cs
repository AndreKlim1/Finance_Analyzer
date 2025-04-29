namespace BudgetingService.Messaging.DTO
{
    public record TransactionFilterParameters(
        int Page,
        long UserId,
        string? SearchTerm,
        DateTime? StartDate,
        DateTime? EndDate,
        string? Categories,
        string? Types,
        decimal? MinValue,
        decimal? MaxValue,
        string? Accounts);
}
