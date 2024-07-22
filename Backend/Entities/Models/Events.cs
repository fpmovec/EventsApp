namespace Entities.Models
{
    public class DetailsChangedEvent
    {
        public string Id { get; set; }
        public virtual string Message { get; set; } = string.Empty;
        public int EventId { get; set; }
        public string[] UserIds { get; set; } = Array.Empty<string>();
    }
}
