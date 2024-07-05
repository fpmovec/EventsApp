namespace Web.ViewModels
{
    public class UserBrief
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly Birthday { get; set; }
    }
}
