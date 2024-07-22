using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Path { get; set; }

        public int EventId { get; set; }
    }
}
