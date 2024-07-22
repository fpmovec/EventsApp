using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
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
