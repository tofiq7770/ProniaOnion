using FluentValidation;
using ProniaOnion.Application.DTOs.Users;

namespace ProniaOnion.Application.Validators
{
    public class RegisterDtoValidator:AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MinimumLength(8).MaximumLength(256).EmailAddress().Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(50);

            RuleFor(x => x.UserName).NotEmpty().MinimumLength(4).MaximumLength(50);

            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);

            RuleFor(x => x.Surname).NotEmpty().MinimumLength(3).MaximumLength(50);

            RuleFor(x => x).Must(X => X.ConfirmPassword == X.Password);

        }
    }
}
