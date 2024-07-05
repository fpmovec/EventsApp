using Application.CollectionServices;
using Application.Models;
using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;

namespace Infrastructure.CollectionServices.Filter
{
    public class BookingFilterService : IFilterService<Booking>
    {
        public FrozenDictionary<FilterType, Func<Booking, object, bool>> Functors => throw new NotImplementedException();

        public IQueryable<Booking> Filter(IQueryable<Booking> collection, FilterType property, object filterValue)
        {
            return collection;
        }

        public IQueryable<Booking> FilterWithManyOptions(IQueryable<Booking> collection, List<FilterOption> filterOptions)
        {
            return collection;
        }
    }
}
