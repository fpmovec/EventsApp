using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum SortProperty
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

    public enum FilterProperty
    {
        Default = 0,
        ByCategory,
        ByDate,
        ByPrice
    }
}
