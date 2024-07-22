using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string EventName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int EventId { get; set; }

        public string UserId { get; set; }

        public double PricePerOne { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public int PersonsQuantity { get; set; }

        public string Email { get; set; }

        public DateOnly Birthday { get; set; }

        [NotMapped]
        public double TotalPrice 
            => PricePerOne * PersonsQuantity;
    }
}
