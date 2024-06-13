using Application.CollectionServices;
using Application.Models;
using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;

namespace Infrastructure.CollectionServices.Filter
{
    public class CategoriesFilterService : IFilterService<EventCategory>
    {
        public FrozenDictionary<FilterType, Func<EventCategory, object, bool>> Functors => throw new NotImplementedException();

        public IQueryable<EventCategory> Filter(IQueryable<EventCategory> collection, FilterType property, object filterValue)
        {
            return collection;
        }

        public IQueryable<EventCategory> FilterWithManyOptions(IQueryable<EventCategory> collection, List<FilterOption> filterOptions)
        {
            return collection;
        }
    }
}
