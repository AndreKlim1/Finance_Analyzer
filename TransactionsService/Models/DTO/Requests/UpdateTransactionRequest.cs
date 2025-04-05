namespace TransactionsService.Models.DTO.Requests
{
    public record UpdateTransactionRequest(
        long Id,
        decimal Value,
        string Title,
        string Currency,
        long CategoryId,
        long AccountId,
        long UserId,
        string? Description,
        string? Image,
        DateTime TransactionDate,
        DateTime CreationDate,
        string TransactionType,
        string? Merchant);
}
