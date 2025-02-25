using FluentValidation;
using UsersService.Models.DTO.Requests;

namespace UsersService.Services.Validators
{
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator() 
        {
            RuleFor(t => t.Id)
               .GreaterThan(0)
               .WithMessage("Id must be greater than 0.");
            RuleFor(profile => profile.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(2, 32).WithMessage("First name must be between 2 and 32 characters");
            RuleFor(profile => profile.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Length(2, 32).WithMessage("Last name must be between 2 and 32 characters");
        }
    }
}
