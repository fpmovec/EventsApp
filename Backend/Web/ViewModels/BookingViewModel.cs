namespace Web.ViewModels
{
    public record BookingViewModel
    {
        public Guid EventId { get; set; }
        public string UserId { get; set; }
        public int PersonsQuantity { get; set; }
        public double Price { get; set; }
    }
}
