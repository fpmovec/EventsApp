using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Phone {  get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}
