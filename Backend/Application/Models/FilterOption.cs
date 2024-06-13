using Domain.Enums;

namespace Application.Models
{
    public class FilterOption
    {
        public FilterType FilterType { get; set; }
        public object? Value { get; set; } = null;
    }
}
