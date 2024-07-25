using Domain.Enums;

namespace Web.DTO
{
    public record FilterOptionsDTO
    {
        public SortType SortType { get; set; } = SortType.Default;
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
        public string? Category { get; set; } = null;
        public DateTime? MinDate { get; set; } = null;
        public DateTime? MaxDate { get; set; } = null;
        public double? MaxPrice { get; set; } = null;
        public double? MinPrice { get; set; } = null;
        public string? Place { get; set; } = null;
        public string? SearchString { get; set; } = null;   
        public int CurrentPage { get; set; } = 1;
    }
}
