using BudgetingService.Models.Enums;

namespace BudgetingService.Models.DTO.Responses
{
    public record BudgetResponse(
        long Id,
        long UserId,
        long? CategoryId,
        string BudgetName,
        string Description,
        int PlannedAmount,
        string Currency,
        DateTime PeriodStart,
        DateTime PeriodEnd,
        string BudgetStatus,
        string BudgetType,
        int WarningThreshold);
}
