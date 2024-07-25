using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;

namespace Application.CollectionServices.Sort
{
    public class TokensSortService : ISortService<RefreshToken>
    {
        public FrozenDictionary<SortType, Func<RefreshToken, object>> Functors => throw new NotImplementedException();

        public IQueryable<RefreshToken> Sort(IQueryable<RefreshToken> collection, SortType sortType, SortOrder order)
        {
            return collection;
        }
    }
}
