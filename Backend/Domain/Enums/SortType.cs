using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum SortType
    {
        [Display(Name = "Default")]
        Default = 0,

        [Display(Name = "By date")]
        ByDate,

        [Display(Name = "By name")]
        ByName,

        [Display(Name = "By price")]
        ByPrice
    }

    public enum FilterType
    {
        Default = 0,
        ByCategory,
        ByMaxDate,
        ByMinDate,
        ByMinPrice,
        ByMaxPrice,
        ByPlace,
    }

    public enum SortOrder
    {
        [Display(Name = "Ascending")]
        Ascending,

        [Display(Name = "Descending")]
        Descending
    }
}
