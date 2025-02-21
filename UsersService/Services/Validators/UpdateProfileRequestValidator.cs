using FluentValidation;
using UsersService.Models.DTO.Requests;

namespace UsersService.Services.Validators
{
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator() 
        {
            RuleFor(profile => profile.Id)
                .NotEmpty();
            RuleFor(profile => profile.FirstName)
                .NotEmpty()
                .Length(2, 32);
            RuleFor(profile => profile.LastName)
                .NotEmpty()
                .Length(2, 32);
        }
    }
}
