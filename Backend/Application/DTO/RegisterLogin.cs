using System.ComponentModel.DataAnnotations;

namespace Web.DTO
{
    public record LoginDTO(
        [Required]string Email,
        [Required]string Password);

    public record RegisterDTO(
        [Required]string Name,
        [Required]string Password,
        [Required][EmailAddress]string Email,
        [Required][Phone]string Phone); 
}
