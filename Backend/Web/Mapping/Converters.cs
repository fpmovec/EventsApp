using AutoMapper;
using Entities.Models;
using Web.ViewModels;

namespace Web.Mapping
{
    public class FilterOptionsConverter : ITypeConverter<FilterOptionsViewModel, List<FilterOption>>
    {
        public List<FilterOption> Convert(FilterOptionsViewModel source, List<FilterOption> destination, ResolutionContext context)
        {
            List<FilterOption> filterOptions = new();

            if (!string.IsNullOrEmpty(source.Category))
                filterOptions.Add(new() { FilterType = Entities.Enums.FilterType.ByCategory, Value = source.Category });

            if (source.MinPrice is not null)
                filterOptions.Add(new() { FilterType = Entities.Enums.FilterType.ByMinPrice, Value = source.MinPrice });

            if (source.MaxPrice is not null)
                filterOptions.Add(new() { FilterType = Entities.Enums.FilterType.ByMaxPrice, Value = source.MaxPrice });

            if (source.MinDate is not null)
                filterOptions.Add(new() { FilterType = Entities.Enums.FilterType.ByMinDate, Value = source.MinDate });

            if (source.MaxDate is not null)
                filterOptions.Add(new() { FilterType = Entities.Enums.FilterType.ByMaxDate, Value = source.MaxDate });

            if (!string.IsNullOrEmpty(source.Place))
                filterOptions.Add(new() { FilterType = Entities.Enums.FilterType.ByPlace, Value = source.Place });

            if (!string.IsNullOrEmpty(source.SearchString))
                filterOptions.Add(new() { FilterType = Entities.Enums.FilterType.ByName, Value = source.SearchString });

            return filterOptions;        
        }
    }
}
