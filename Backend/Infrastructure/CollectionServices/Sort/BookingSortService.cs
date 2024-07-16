using Application.CollectionServices;
using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;

namespace Infrastructure.CollectionServices.Sort
{
    public class BookingSortService : ISortService<Booking>
    {

        // TODO: End up with bookings sort
        public FrozenDictionary<SortType, Func<Booking, object>> Functors => throw new NotImplementedException();

        public IQueryable<Booking> Sort(IQueryable<Booking> collection, SortType sortType, SortOrder order)
        {
            return collection;
        }
    }
}
