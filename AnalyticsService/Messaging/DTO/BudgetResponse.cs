
namespace AnalyticsService.Messaging.DTO
{
    public record BudgetResponse(
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
