using FluentValidation;
using UsersService.Models.DTO.Requests;

namespace UsersService.Services.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(u => u.Role)
                .NotEmpty().WithMessage("Role type is required.")
                .MaximumLength(50).WithMessage("Role type must not exceed 50 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");

            RuleFor(u => u.RegistrationDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Registration date cannot be in the future.");

            RuleFor(u => u.ProfileId)
                .GreaterThan(0).WithMessage("ProfileId must be greater than 0.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
        }
    }
}
