using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public record EventViewModel
    {
        [Required]
        [MinLength(2), MaxLength(60)]
        public string Name { get; set; }

        [Required]
        [MinLength(2), MaxLength(int.MaxValue)]
        public string Description { get; set; }

        [Required]
        [MinLength(2), MaxLength(150)]
        public string Place {  get; set; }

        [Required]
        [Range(0, 5000.0)]
        public double Price { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxParticipantsCount { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public IFormFile? ImageFile {  get; set; } 
    }

    public record ImageInfo(string Name, string Path);
}
