using FluentValidation;
using Web.DTO;

namespace Application.FluentValidation
{
    public class TokenRequestValidator : AbstractValidator<TokenRequest>
    {
        public TokenRequestValidator()
        {
            RuleFor(t => t.MainToken)
                .NotEmpty();

            RuleFor(t => t.RefreshToken)
                .NotEmpty();
        }
    }
}
