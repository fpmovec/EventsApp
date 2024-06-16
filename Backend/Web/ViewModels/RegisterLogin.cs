using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public record LoginViewModel(
        [Required]string Email,
        [Required]string Password);

    public record RegisterViewModel(
        [Required]string UserName,
        [Required]string Password,
        [Required][EmailAddress]string Email); 
}
