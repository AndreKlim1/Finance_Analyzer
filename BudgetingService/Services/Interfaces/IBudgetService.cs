using BudgetingService.Models.DTO.Requests;
using BudgetingService.Models.DTO.Responses;
using BudgetingService.Models.Errors;


namespace BudgetingService.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<Result<BudgetResponse>> GetBudgetByIdAsync(long id, CancellationToken token);
        Task<Result<List<BudgetResponse>>> GetBudgetsAsync(CancellationToken token);
        Task<Result<BudgetResponse>> CreateBudgetAsync(CreateBudgetRequest createBudgetRequest, CancellationToken token);
        Task<Result<BudgetResponse>> UpdateBudgetAsync(UpdateBudgetRequest updateBudgetRequest, CancellationToken token);
        Task<bool> DeleteBudgetAsync(long id, CancellationToken token);
        Task<Result<List<BudgetResponse>>> GetBudgetsByUserIdAsync(long userId, CancellationToken token);
    }
}
