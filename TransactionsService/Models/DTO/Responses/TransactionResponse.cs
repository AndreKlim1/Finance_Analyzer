using TransactionsService.Models.Enums;

namespace TransactionsService.Models.DTO.Responses
{
    public record TransactionResponse(
        long Id,
        long Value,
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
