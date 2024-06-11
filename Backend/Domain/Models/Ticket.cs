using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid EventId { get; set; }

        public Guid ParticipantId { get; set; }

        public double PricePerOne { get; set; }

        public int TicketsCount { get; set; }

        [NotMapped]
        public double TotalPrice 
            => PricePerOne * TicketsCount;
    }
}
