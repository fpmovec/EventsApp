using FluentValidation;
using Web.DTO;

namespace Application.FluentValidation
{
    public class BookingDTOValidator : AbstractValidator<BookingDTO>
    {
        public BookingDTOValidator()
        {
            RuleFor(b => b.EventId)
                 .NotEmpty();

            RuleFor(b => b.UserId)
                .NotEmpty();

            RuleFor(b => b.FullName)
                .NotEmpty();

            RuleFor(b => b.PersonsQuantity)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .LessThan(int.MaxValue);

            RuleFor(b => b.Phone)
                .NotEmpty();

            RuleFor(b => b.Email)
                .NotEmpty();

            RuleFor(b => b.Birthday)
                .NotEmpty()
                .LessThan(DateOnly.FromDateTime(DateTime.Now));
        }
    }
}
