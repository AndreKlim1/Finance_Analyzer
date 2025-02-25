namespace BudgetingService.Models.DTO.Requests
{
    public record UpdateBudgetRequest(
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
