using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class User111
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FullName
            => $"{FirstName} {LastName}";

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Login { get; set; }

        public string? HashedPassword { get; set; }
    }

    public class UserResponse
    {
        public string Id { get; set; }

        public string? Email { get; set;}

        public string? Name { get; set;}
    }
}
