using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum ExceptionSubject
    {
        [Display(Name = "Category")]
        Category = 0,

        [Display(Name = "Event")]
        Event,

        [Display(Name = "User")]
        User,

        [Display(Name = "Booking")]
        Booking
    }
}
