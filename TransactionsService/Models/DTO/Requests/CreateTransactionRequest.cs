namespace TransactionsService.Models.DTO.Requests
{
    public record CreateTransactionRequest(
        int Value,
        string Currency,
        long CategoryId,
        long AccountId,
        long UserId,
        string? Description,
        string? Image,
        DateTime TransactionDate,
        DateTime CreationDate,
        string PaymentMethod,
        string? Merchant);
}
