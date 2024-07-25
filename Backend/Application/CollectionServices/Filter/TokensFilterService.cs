using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;

namespace Application.CollectionServices.Filter
{
    public class TokensFilterService : IFilterService<RefreshToken>
    {
        public FrozenDictionary<FilterType, Func<RefreshToken, object, bool>> Functors => throw new NotImplementedException();

        public IQueryable<RefreshToken> Filter(IQueryable<RefreshToken> collection, FilterType property, object filterValue)
        {
            return collection;
        }

        public IQueryable<RefreshToken> FilterWithManyOptions(IQueryable<RefreshToken> collection, List<FilterOption> filterOptions)
        {
            return collection;
        }
    }
}
