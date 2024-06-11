using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Participant
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public DateOnly BirthdayDate { get; set; }
    }
}
