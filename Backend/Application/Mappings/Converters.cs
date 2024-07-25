using AutoMapper;
using Domain.Models;
using Web.DTO;

namespace Application.Mappings
{
    public class FilterOptionsConverter : ITypeConverter<FilterOptionsDTO, List<FilterOption>>
    {
        public List<FilterOption> Convert(FilterOptionsDTO source, List<FilterOption> destination, ResolutionContext context)
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

            if (!string.IsNullOrEmpty(source.SearchString))
                filterOptions.Add(new() { FilterType = Domain.Enums.FilterType.ByName, Value = source.SearchString });

            return filterOptions;
        }
    }
}
