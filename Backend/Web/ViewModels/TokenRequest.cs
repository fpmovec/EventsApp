using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public record TokenRequest(
        [Required] string MainToken,
        [Required] string RefreshToken
    );
}
