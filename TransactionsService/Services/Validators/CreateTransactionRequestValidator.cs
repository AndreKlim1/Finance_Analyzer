using FluentValidation;
using TransactionsService.Models.DTO.Requests;
using TransactionsService.Models.Enums;

namespace TransactionsService.Services.Validators
{
    public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionRequestValidator()
        {
            RuleFor(t => t.Value)
                .NotEmpty().WithMessage("Transaction value must not be empty.");

            RuleFor(t => t.Value)
                .GreaterThan(0).WithMessage("Value must be greater than 0")
                .When(t => t.TransactionType == TransactionType.INCOME.ToString());

            RuleFor(t => t.Value)
                .LessThan(0).WithMessage("Value must be less than 0")
                .When(t => t.TransactionType == TransactionType.EXPENSE.ToString());

            RuleFor(t => t.Title)
                .MaximumLength(128).WithMessage("Title must not exceed 128 characters");

            RuleFor(t => t.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .MaximumLength(3).WithMessage("Currency code must be at most 3 characters long.");

            RuleFor(t => t.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0.");

            RuleFor(t => t.AccountId)
                .GreaterThan(0).WithMessage("AccountId must be greater than 0.");

            RuleFor(t => t.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(t => t.Description)
                .MaximumLength(512).WithMessage("Description must not exceed 512 characters.");

            RuleFor(t => t.Image)
                .MaximumLength(256).WithMessage("Image URL must not exceed 256 characters.");

            RuleFor(t => t.TransactionDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transaction date cannot be in the future.");

            RuleFor(t => t.CreationDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Creation date cannot be in the future.");

            RuleFor(t => t.TransactionType)
                .NotEmpty().WithMessage("Payment method is required.")
                .MaximumLength(64).WithMessage("Payment method must not exceed 50 characters.");

            RuleFor(t => t.Merchant)
                .MaximumLength(128).WithMessage("Merchant name must not exceed 128 characters.");
        }
    }
}
