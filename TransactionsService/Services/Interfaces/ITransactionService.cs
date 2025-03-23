using TransactionsService.Models;
using TransactionsService.Models.DTO.Requests;
using TransactionsService.Models.DTO.Responses;
using TransactionsService.Models.Errors;


namespace TransactionsService.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Result<TransactionResponse>> GetTransactionByIdAsync(long id, CancellationToken token);
        Task<Result<List<TransactionResponse>>> GetTransactionsAsync(CancellationToken token);
        Task<Result<List<TransactionResponse>>> GetTransactionsByUserIdAsync(long userId, CancellationToken token);
        Task<Result<TransactionResponse>> CreateTransactionAsync(CreateTransactionRequest createTransactionRequest, CancellationToken token);
        Task<Result<TransactionResponse>> CreateTransactionFromAccountAsync(CreateTransactionRequest createTransactionRequest, CancellationToken token);
        Task<Result<TransactionResponse>> UpdateTransactionAsync(UpdateTransactionRequest updateTransactionRequest, CancellationToken token);
        Task<bool> DeleteTransactionAsync(long id, CancellationToken token);
        Task<bool> DeleteTransactionsByAccountIdAsync(long accountId, CancellationToken token);
        Task<bool> DeleteTransactionsByCategoryIdAsync(long categoryId, CancellationToken token);
    }
}
