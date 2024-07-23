using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public record BookingViewModel
    {
        public int EventId { get; set; }

        public string UserId { get; set; }

        public string FullName { get; set; }
        
        [Phone]
        public string Phone {  get; set; }

        public int PersonsQuantity { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateOnly Birthday { get; set; }
    }
}
