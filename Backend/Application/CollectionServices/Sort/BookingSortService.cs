using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;

namespace Application.CollectionServices.Sort
{
    public class BookingSortService : ISortService<Booking>
    {
        public FrozenDictionary<SortType, Func<Booking, object>> Functors => throw new NotImplementedException();

        public IQueryable<Booking> Sort(IQueryable<Booking> collection, SortType sortType, SortOrder order)
        {
            return collection;
        }
    }
}
