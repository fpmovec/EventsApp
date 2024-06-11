using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
    public class EventBaseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string BriefDescription
            => Description[50..];
        
        public string Place {  get; set; } = string.Empty; 

        public DateOnly Date {  get; set; }

        public TimeOnly Time { get; set; }

        public EventType EventType { get; set; }

        public IFormFile Image {  get; set; }
    }

    public class EventExtendedModel : EventBaseModel
    {
        public int MaxParticipantsCount { get; set; }

        public int BookedTicketsCount { get; set; }

        public int RemainingTicketsCount
            => MaxParticipantsCount - BookedTicketsCount;

        public bool IsSoldOut
            => RemainingTicketsCount == 0;

        public ICollection<Participant> Participants { get; set; }
    }
}
