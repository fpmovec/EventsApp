using Microsoft.AspNetCore.Http;

namespace Web.DTO
{
    public record EventDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Place {  get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        public int MaxParticipantsCount { get; set; }

        public string CategoryName { get; set; }

        public IFormFile? ImageFile {  get; set; } 
    }

    public record ImageInfo(string Name, string Path);
}
