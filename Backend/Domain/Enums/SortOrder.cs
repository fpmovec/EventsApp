using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum SortOrder
    {
        [Display(Name = "Ascending")]
        Ascending,

        [Display(Name = "Descending")]
        Descending
    }
}
