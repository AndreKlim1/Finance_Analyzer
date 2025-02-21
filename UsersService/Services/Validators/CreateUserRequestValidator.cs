using FluentValidation;
using UsersService.Models.DTO.Requests;

namespace UsersService.Services.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(64);
            RuleFor(user => user.Password)
                .NotEmpty();
            RuleFor(user => user.Role)
                .NotEmpty();
            RuleFor(user => user.RegistrationDate)
                .NotNull();
            RuleFor(user => user.ProfileId)
                .NotEmpty();
        }
    }
}
