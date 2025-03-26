namespace BudgetingService.Models.DTO.Requests
{
    public record CreateBudgetRequest(
        long UserId,
        long? CategoryId,
        long? AccountId,
        string BudgetName,
        string Description,
        int PlannedAmount,
        int CurrValue,
        string Currency,
        DateTime PeriodStart,
        DateTime PeriodEnd,
        string BudgetStatus,
        string BudgetType,
        int WarningThreshold,
        bool WarningShowed,
        string Color);
}
