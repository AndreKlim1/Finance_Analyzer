namespace BudgetingService.Models.DTO.Requests
{
    public record CreateBudgetRequest(
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
