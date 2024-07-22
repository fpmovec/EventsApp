using Entities.Models;

namespace Application.ViewModels
{
    public class FilteredEventsResponse
    {
        public ICollection<EventBaseModel> Events { get; set; }

        public int Pages { get; set; }
    }
}
