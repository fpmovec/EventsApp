using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

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

        public DateOnly Date {  get; set; }

        public TimeOnly Time { get; set; }

        public EventCategory Category { get; set; }

        public double Price { get; set; }

        public Image Image {  get; set; }

        public int CategoryId { get; set; }
    }

    public class EventExtendedModel : EventBaseModel
    {
        public int MaxParticipantsCount { get; set; }

        public int BookedTicketsCount { get; set; }

        public int RemainingTicketsCount
            => MaxParticipantsCount - BookedTicketsCount;

        [NotMapped]
        public bool IsSoldOut
            => RemainingTicketsCount == 0;

        public ICollection<Participant> Participants { get; set; }
    }
}
