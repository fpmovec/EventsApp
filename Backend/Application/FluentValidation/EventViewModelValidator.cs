using FluentValidation;
using Web.ViewModels;

namespace Application.FluentValidation
{
    public class EventViewModelValidator : AbstractValidator<EventViewModel>
    {
        public EventViewModelValidator()
        {
            RuleFor(e => e.Name)
               .NotEmpty()
               .Length(2, 60);

            RuleFor(e => e.Description)
                .NotEmpty()
                .Length(2, int.MaxValue);

            RuleFor(e => e.Place)
                .NotEmpty()
                .Length(2, 150);

            RuleFor(e => e.Price)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(5000);

            RuleFor(e => e.MaxParticipantsCount)
                .NotEmpty()
                .GreaterThan(0)
                .LessThan(int.MaxValue);

            RuleFor(e => e.CategoryName)
                .NotEmpty();
        }
    }
}
