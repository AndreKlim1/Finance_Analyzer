using BudgetingService.Models.DTO.Requests;
using BudgetingService.Models.DTO.Responses;
using BudgetingService.Models.Errors;


namespace BudgetingService.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<Result<BudgetResponse>> GetTransactionByIdAsync(long id, CancellationToken token);
        Task<Result<BudgetResponse>> GetTransactionByEmailAsync(string email, CancellationToken token);
        Task<Result<BudgetResponse>> CreateTransactionAsync(CreateBudgetRequest createTransactionRequest, CancellationToken token);
        Task<Result<BudgetResponse>> UpdateTransactionAsync(UpdateBudgetRequest updateTransactionRequest, CancellationToken token);
        Task<bool> DeleteTransactionAsync(long id, CancellationToken token);
    }
}
