using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public record BookingViewModel
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string FullName { get; set; }
        
        [Required]
        [Phone]
        public string Phone {  get; set; }

        [Required, Range(1, int.MaxValue)]
        public int PersonsQuantity { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateOnly Birthday { get; set; }
    }
}
