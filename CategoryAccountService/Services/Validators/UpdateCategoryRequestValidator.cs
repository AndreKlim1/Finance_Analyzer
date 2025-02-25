using FluentValidation;
using CaregoryAccountService.Models.DTO.Requests;

namespace CaregoryAccountService.Services.Validators
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(t => t.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");

            RuleFor(c => c.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(c => c.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            RuleFor(c => c.CategoryType)
                .NotEmpty().WithMessage("Category type is required.")
                .MaximumLength(50).WithMessage("Category type must not exceed 50 characters.");
        }
    }
}
