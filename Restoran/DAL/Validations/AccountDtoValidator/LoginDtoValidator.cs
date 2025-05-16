using FluentValidation;
using Restoran.DTOs;

namespace Restoran.DAL.Validatos.AccountValidator
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .Length(5, 50).WithMessage("UserName must be between 5 and 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
