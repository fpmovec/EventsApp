﻿using Microsoft.Extensions.Options;
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
}
