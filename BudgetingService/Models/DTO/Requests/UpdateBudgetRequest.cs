namespace BudgetingService.Models.DTO.Requests
{
    public record UpdateBudgetRequest(
        long Id,
        long UserId,
        List<long>? CategoryIds,
        List<long>? AccountIds,
        string BudgetName,
        string Description,
        decimal PlannedAmount,
        decimal CurrValue,
        string Currency,
        DateTime PeriodStart,
        DateTime PeriodEnd,
        string BudgetStatus,
        string BudgetType,
        int WarningThreshold,
        bool WarningShowed,
        string Color);
}
