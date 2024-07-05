using Application.CollectionServices;
using Application.Models;
using Domain.Enums;
using Domain.Models;
using System.Collections.Frozen;

namespace Infrastructure.CollectionServices.Filter
{
    public class ParticipantsFilterService : IFilterService<Participant>
    {
        public FrozenDictionary<FilterType, Func<Participant, object, bool>> Functors => throw new NotImplementedException();

        public IQueryable<Participant> Filter(IQueryable<Participant> collection, FilterType property, object filterValue)
        {
            return collection;
        }

        public IQueryable<Participant> FilterWithManyOptions(IQueryable<Participant> collection, List<FilterOption> filterOptions)
        {
            return collection;
        }
    }
}
