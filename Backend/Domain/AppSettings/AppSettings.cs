using System.ComponentModel.DataAnnotations;

namespace Domain.AppSettings
{
    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
    {
        [Required]
        public string? SecretKey { get; set; }

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
}
