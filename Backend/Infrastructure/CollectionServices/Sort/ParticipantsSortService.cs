using Application.CollectionServices;
using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;

namespace Infrastructure.CollectionServices.Sort
{
    public class ParticipantsSortService : ISortService<Participant>
    {
        public FrozenDictionary<SortType, Func<Participant, object>> Functors => throw new NotImplementedException();

        public IQueryable<Participant> Sort(IQueryable<Participant> collection, SortType sortType, SortOrder order)
        {
            return collection;
        }
    }
}
