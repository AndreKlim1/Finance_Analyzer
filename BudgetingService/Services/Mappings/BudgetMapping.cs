using System.Runtime.CompilerServices;
using BudgetingService.Models;
using BudgetingService.Models.DTO.Requests;
using BudgetingService.Models.DTO.Responses;
using BudgetingService.Models.Enums;


namespace BudgetingService.Services.Mappings
{
    public static class BudgetMapping
    {
        public static BudgetResponse ToBudgetResponse(this Budget budget)
        {
            return new BudgetResponse
            (
                budget.Id,
                budget.UserId,
                budget.CategoryId,
                budget.AccountId,
                budget.BudgetName,
                budget.Description,
                budget.PlannedAmount,
                budget.CurrValue,
                budget.Currency.ToString(),
                budget.PeriodStart,
                budget.PeriodEnd,
                budget.BudgetStatus.ToString(),
                budget.BudgetType.ToString(),
                budget.WarningThreshold,
                budget.WarningShowed,
                budget.Color
            );
        }

        public static Budget ToBudget(this CreateBudgetRequest request)
        {
            return new Budget
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                AccountId = request.AccountId,
                BudgetName = request.BudgetName,
                Description = request.Description,
                PlannedAmount = request.PlannedAmount,
                CurrValue = request.CurrValue,
                Currency = Enum.Parse<Currency>(request.Currency),
                PeriodStart = request.PeriodStart,
                PeriodEnd = request.PeriodEnd,
                BudgetStatus = Enum.Parse<BudgetStatus>(request.BudgetStatus),
                BudgetType = Enum.Parse<BudgetType>(request.BudgetType),
                WarningThreshold = request.WarningThreshold,
                WarningShowed = request.WarningShowed,
                Color = request.Color
            };
        }

        public static Budget ToBudget(this UpdateBudgetRequest request)
        {
            return new Budget
            {
                Id = request.Id,
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                AccountId = request.AccountId,
                BudgetName = request.BudgetName,
                Description = request.Description,
                PlannedAmount = request.PlannedAmount,
                CurrValue = request.CurrValue,
                Currency = Enum.Parse<Currency>(request.Currency),
                PeriodStart = request.PeriodStart,
                PeriodEnd = request.PeriodEnd,
                BudgetStatus = Enum.Parse<BudgetStatus>(request.BudgetStatus),
                BudgetType = Enum.Parse<BudgetType>(request.BudgetType),
                WarningThreshold = request.WarningThreshold,
                WarningShowed = request.WarningShowed,
                Color = request.Color
            };
        }
    }
}
