using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Path { get; set; }

        public int EventId { get; set; }
    }
}
