using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
{
    public enum SortOrder
    {
        [Display(Name = "Ascending")]
        Ascending,

        [Display(Name = "Descending")]
        Descending
    }
}
