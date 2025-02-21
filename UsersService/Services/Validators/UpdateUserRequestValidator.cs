using FluentValidation;
using UsersService.Models.DTO.Requests;

namespace UsersService.Services.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator() 
        {
            RuleFor(user => user.Id)
                .NotEmpty();
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
