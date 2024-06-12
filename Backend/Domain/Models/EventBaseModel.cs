using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public abstract class EventBaseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string BriefDescription
            => Description[50..];
        
        public string Place {  get; set; } = string.Empty; 

        public DateOnly Date {  get; set; } = default(DateOnly);

        public TimeOnly Time { get; set; } = default(TimeOnly);

        public EventCategory Category { get; set; }

        public double Price { get; set; }

        public Image Image { get; set; } = null;

        public int CategoryId { get; set; }
    }

    public class EventExtendedModel : EventBaseModel
    {
        public int MaxParticipantsCount { get; set; }

        [JsonIgnore]
        public int BookedTicketsCount { get; set; }

        [JsonIgnore]
        public int RemainingTicketsCount
            => MaxParticipantsCount - BookedTicketsCount;

        [NotMapped]
        public bool IsSoldOut
            => RemainingTicketsCount == 0;

        public ICollection<Participant> Participants { get; set; }
    }
}
