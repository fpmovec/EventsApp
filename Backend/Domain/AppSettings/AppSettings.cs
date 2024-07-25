using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Domain.AppSettings
{
    public class AppSettings
    {
        [Required]
        [ValidateObjectMembers]
        public JwtSettings JwtSettings { get; set; }

        [Required]
        [ValidateObjectMembers]
        public PaginationSettings PaginationSettings { get; set; }

        [Required]
        public EventsSettings EventsSettings { get; set; }

        public EventMessages EventMessages { get; set; }
    }

    public class JwtSettings
    {
        [Required]
        public string SecretKey { get; set; } = string.Empty;

        [Required]
        public string? Audience { get; set; }

        [Required]
        public string? Authority { get; set; }

        [Required]
        public TimeSpan ExpirationTimeFrame { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int RefreshTokenExpirationInHours { get; set; }
    }

    public class SignalRSettings
    {

    }

    public class PaginationSettings
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
    }
    public class EventsSettings
    {
        [Required]
        public int MinEventPrice { get; set; }

        [Required]
        public int MaxEventPrice { get; set; }
    }

    public class EventMessages
    {
        public string ChangedEventInfoMessage { get; set; } = string.Empty;
    }
}