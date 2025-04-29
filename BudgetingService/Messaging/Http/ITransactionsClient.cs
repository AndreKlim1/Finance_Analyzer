using BudgetingService.Messaging.DTO;

namespace BudgetingService.Messaging.Http
{
    public interface ITransactionsClient
    {
        Task<List<TransactionResponse>> GetTransactionsAsync(TransactionFilterParameters filter, CancellationToken cancellationToken);
    }
}
