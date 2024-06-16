using System.ComponentModel.DataAnnotations;

namespace Domain.AppSettings
{
    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; }

        public PaginationSettings PaginationSettings { get; set; }
    }

    public class JwtSettings
    {
        [Required]
        public string SecretKey { get; set; } = string.Empty;

        [Required]
        public string? Audience {  get; set; }

        [Required]
        public string? Authority { get; set; }

        [Required]
        public int ExpirationTimeInHours { get; set; }
    }

    public class SignalRSettings
    {

    }

    public class PaginationSettings
    {
        [Required]
        public int PageSize { get; set; }
    }
}
