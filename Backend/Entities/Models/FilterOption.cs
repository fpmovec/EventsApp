using Entities.Enums;

namespace Entities.Models
{
    public class FilterOption
    {
        public FilterType FilterType { get; set; }
        public object? Value { get; set; } = null;
    }
}
