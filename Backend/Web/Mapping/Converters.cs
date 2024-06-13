using Application.Models;
using AutoMapper;
using Web.ViewModels;

namespace Web.Mapping
{
    public class FilterOptionsConverter : ITypeConverter<FilterOptionsViewModel, List<FilterOption>>
    {
        public List<FilterOption> Convert(FilterOptionsViewModel source, List<FilterOption> destination, ResolutionContext context)
        {
            List<FilterOption> filterOptions = new();

            if (!string.IsNullOrEmpty(source.Category))
                filterOptions.Add(new() { FilterType = Domain.Enums.FilterType.ByCategory, Value = source.Category });

            if (source.MinPrice is not null)
                filterOptions.Add(new() { FilterType = Domain.Enums.FilterType.ByMinPrice, Value = source.MinPrice });

            if (source.MaxPrice is not null)
                filterOptions.Add(new() { FilterType = Domain.Enums.FilterType.ByMaxPrice, Value = source.MaxPrice });

            if (source.MinDate is not null)
                filterOptions.Add(new() { FilterType = Domain.Enums.FilterType.ByMinDate, Value = source.MinDate });

            if (source.MaxDate is not null)
                filterOptions.Add(new() { FilterType = Domain.Enums.FilterType.ByMaxDate, Value = source.MaxDate });

            if (!string.IsNullOrEmpty(source.Place))
                filterOptions.Add(new() { FilterType = Domain.Enums.FilterType.ByPlace, Value = source.Place });

            return filterOptions;        
        }
    }
}
