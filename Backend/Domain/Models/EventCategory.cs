namespace Domain.Models
{
    public record EventCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
