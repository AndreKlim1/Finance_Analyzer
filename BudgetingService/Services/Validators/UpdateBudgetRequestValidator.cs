using FluentValidation;
using BudgetingService.Models.DTO.Requests;

namespace BudgetingService.Services.Validators
{
    
    public class UpdateBudgetRequestValidator : AbstractValidator<UpdateBudgetRequest>
    {
        public UpdateBudgetRequestValidator()
        {
            RuleFor(t => t.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");

            RuleFor(b => b.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(b => b.CategoryId)
                .GreaterThan(0).When(b => b.CategoryId.HasValue).WithMessage("CategoryId must be greater than 0 if provided.");

            RuleFor(b => b.AccountId)
               .GreaterThan(0).When(b => b.AccountId.HasValue).WithMessage("AccountId must be greater than 0 if provided.");

            RuleFor(b => b.BudgetName)
                .NotEmpty().WithMessage("Budget name is required.")
                .MaximumLength(100).WithMessage("Budget name must not exceed 100 characters.");

            RuleFor(b => b.Description)
                .MaximumLength(512).WithMessage("Description must not exceed 512 characters.");

            RuleFor(b => b.PlannedAmount)
                .GreaterThan(0).WithMessage("Planned amount must be greater than 0.");
            
            RuleFor(b => b.CurrValue)
                .GreaterThanOrEqualTo(0).WithMessage("Current value must be grater or equal to zero");

            RuleFor(b => b.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .MaximumLength(3).WithMessage("Currency code must be at most 3 characters long.");

            RuleFor(b => b.PeriodStart)
                .LessThan(b => b.PeriodEnd).WithMessage("Period start must be before period end.");

            RuleFor(b => b.PeriodEnd)
                .GreaterThan(b => b.PeriodStart).WithMessage("Period end must be after period start.");

            RuleFor(b => b.BudgetStatus)
                .NotEmpty().WithMessage("Budget status is required.")
                .MaximumLength(50).WithMessage("Budget status must not exceed 50 characters.");

            RuleFor(b => b.BudgetType)
                .NotEmpty().WithMessage("Budget type is required.")
                .MaximumLength(50).WithMessage("Budget type must not exceed 50 characters.");

            RuleFor(b => b.WarningThreshold)
                .InclusiveBetween(0, 100).WithMessage("Warning threshold must be between 0 and 100.");

            RuleFor(b => b.WarningShowed)
                .NotEmpty();
        }
    }

}
