using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
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
}
