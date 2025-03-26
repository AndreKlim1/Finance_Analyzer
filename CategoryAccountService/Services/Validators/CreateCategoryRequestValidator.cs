using FluentValidation;
using CaregoryAccountService.Models.DTO.Requests;

namespace CaregoryAccountService.Services.Validators
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(c => c.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(c => c.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            RuleFor(c => c.CategoryType)
                .NotEmpty().WithMessage("Category type is required.")
                .MaximumLength(50).WithMessage("Category type must not exceed 50 characters.");

            RuleFor(a => a.Icon)
                .NotEmpty().WithMessage("Icon must not be empty");
        }
    }
}
