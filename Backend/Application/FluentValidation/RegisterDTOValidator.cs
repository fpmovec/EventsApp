using FluentValidation;
using Web.DTO;

namespace Application.FluentValidation
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty();

            RuleFor(r => r.Password)
                .NotEmpty();

            RuleFor(r => r.Phone)
                .NotEmpty();

            RuleFor(r => r.Name)
                .NotEmpty();
        }
    }
}
