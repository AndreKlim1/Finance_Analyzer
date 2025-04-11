namespace TransactionsService.Models.DTO.Requests
{
    public record TransactionFilterParameters(
        int Page,
        string? SearchTerm,
        DateTime? StartDate,
        DateTime? EndDate,
        string? Categories,
        string? Types,
        decimal? MinValue,
        decimal? MaxValue,
        string? Accounts);

}
