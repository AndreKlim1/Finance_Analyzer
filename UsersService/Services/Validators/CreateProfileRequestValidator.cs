using FluentValidation;
using UsersService.Models.DTO.Requests;

namespace UsersService.Services.Validators
{
    public class CreateProfileRequestValidator : AbstractValidator<CreateProfileRequest>
    {
        public CreateProfileRequestValidator()
        {
            RuleFor(profile => profile.FirstName)
                .NotEmpty()
                .Length(2, 32);
            RuleFor(profile => profile.LastName)
                .NotEmpty()
                .Length(2, 32);
        }
    }
}
