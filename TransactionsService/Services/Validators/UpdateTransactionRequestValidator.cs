using FluentValidation;
using TransactionsService.Models.DTO.Requests;

namespace UsersService.Services.Validators
{
    public class UpdateTransactionRequestValidator : AbstractValidator<UpdateTransactionRequest>
    {
        public UpdateTransactionRequestValidator()
        {
            RuleFor(t => t.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");

            RuleFor(t => t.Value)
                .GreaterThan(0)
                .WithMessage("Value must be a positive number.");

            RuleFor(t => t.Currency)
                .NotEmpty()
                .WithMessage("Currency must not be empty.")
                .MaximumLength(3)
                .WithMessage("Currency must not exceed 3 characters.");

            RuleFor(t => t.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0.");

            RuleFor(t => t.AccountId)
                .GreaterThan(0)
                .WithMessage("AccountId must be greater than 0.");

            RuleFor(t => t.UserId)
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0.");

            RuleFor(t => t.Description)
                .MaximumLength(512)
                .WithMessage("Description must not exceed 512 characters.");

            RuleFor(t => t.Image)
                .MaximumLength(256)
                .WithMessage("Image must not exceed 256 characters.");

            RuleFor(t => t.TransactionDate)
                .NotEmpty()
                .WithMessage("TransactionDate must not be empty.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("TransactionDate cannot be in the future.");

            RuleFor(t => t.CreationDate)
                .NotEmpty()
                .WithMessage("CreationDate must not be empty.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("CreationDate cannot be in the future.");

            RuleFor(t => t.PaymentMethod)
                .NotEmpty()
                .WithMessage("PaymentMethod must not be empty.")
                .MaximumLength(64)
                .WithMessage("PaymentMethod must not exceed 64 characters.");

            RuleFor(t => t.Merchant)
                .MaximumLength(128)
                .WithMessage("Merchant must not exceed 128 characters.");
        }
    }

}
