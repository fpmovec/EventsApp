using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public abstract class EventBaseModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string BriefDescription
            => Description.Length > 50 ? Description[..50] : Description;
        
        public string Place {  get; set; } = string.Empty; 

        public DateTime Date {  get; set; } = default;

        public EventCategory Category { get; set; }

        public double Price { get; set; }

        public Image Image { get; set; } = null;

        public int CategoryId { get; set; }
    }

    public class EventExtendedModel : EventBaseModel
    {
        public int MaxParticipantsCount { get; set; }

        public int BookedTicketsCount { get; set; } = 0;

        public int RemainingTicketsCount
            => MaxParticipantsCount - BookedTicketsCount;

        [NotMapped]
        public bool IsSoldOut
            => RemainingTicketsCount == 0;
    }
}
