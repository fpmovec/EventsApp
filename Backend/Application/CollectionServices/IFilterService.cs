using Domain.Enums;
using System.Collections.Frozen;

namespace Application.CollectionServices
{
    public interface IFilterService<Tentity> where Tentity : class
    {
        FrozenDictionary<EventFilterType, Func<Tentity, object, bool>> Functors { get; }
        IQueryable<Tentity> Filter(IQueryable<Tentity> collection, EventFilterType property, object filterValue);
    }
}
