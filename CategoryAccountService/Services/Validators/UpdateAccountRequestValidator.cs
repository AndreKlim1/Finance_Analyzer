using FluentValidation;
using CaregoryAccountService.Models.DTO.Requests;

namespace CaregoryAccountService.Services.Validators
{

    public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
    {
        public UpdateAccountRequestValidator()
        {
            RuleFor(t => t.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");

            RuleFor(a => a.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(a => a.AccountName)
                .NotEmpty().WithMessage("Account name is required.")
                .MaximumLength(100).WithMessage("Account name must not exceed 100 characters.");

            RuleFor(a => a.AccountType)
                .NotEmpty().WithMessage("Account type is required.")
                .MaximumLength(50).WithMessage("Account type must not exceed 50 characters.");

            RuleFor(a => a.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .MaximumLength(3).WithMessage("Currency code must be at most 3 characters long.");

            RuleFor(a => a.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("Balance must be zero or greater.");

            RuleFor(a => a.Description)
                .MaximumLength(512).WithMessage("Description must not exceed 512 characters.");
        }
    }
}
