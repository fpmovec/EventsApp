namespace Domain.Models
{
    class Ticket
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid EventId { get; set; }

        public Guid ParticipantId { get; set; }

        public EventExtendedModel Event {  get; set; }

        public Participant Participant { get; set; }
    }
}
