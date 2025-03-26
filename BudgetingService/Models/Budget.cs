using System.Data;
using BudgetingService.Models.Enums;


namespace BudgetingService.Models
{
    public class Budget : BaseModel<long>
    {
        public long UserId { get; set; }
        public long? CategoryId { get; set; }
        public long? AccountId { get; set; }
        public string BudgetName { get; set; }
        public string Description { get; set; }
        public int PlannedAmount { get; set; }
        public int CurrValue { get; set; }
        public Currency Currency { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public BudgetStatus BudgetStatus { get; set; }
        public BudgetType BudgetType { get; set; }
        public int WarningThreshold { get; set; } 
        public bool WarningShowed { get; set; }
        public string Color { get; set; }
    }
}
