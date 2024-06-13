using Application.Models;
using Domain.Enums;
using System.Collections.Frozen;

namespace Application.CollectionServices
{
    public interface IFilterService<Tentity> where Tentity : class
    {
        FrozenDictionary<FilterType, Func<Tentity, object, bool>> Functors { get; }
        IQueryable<Tentity> Filter(IQueryable<Tentity> collection, FilterType property, object filterValue);
        IQueryable<Tentity> FilterWithManyOptions(IQueryable<Tentity> collection, List<FilterOption> filterOptions);
    }
}
