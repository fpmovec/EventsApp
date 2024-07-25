using FluentValidation;
using Web.DTO;

namespace Application.FluentValidation
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(l => l.Email)
                .NotEmpty();

            RuleFor(l => l.Password)
                .NotEmpty();
        }
    }
}
