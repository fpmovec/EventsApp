using System.ComponentModel.DataAnnotations;

namespace Web.DTO
{
    public record TokenRequest(
        [Required] string MainToken,
        [Required] string RefreshToken
    );
}
