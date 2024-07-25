using Domain.Enums;
using System.Collections.Frozen;

namespace Application.CollectionServices
{
    public interface ISortService<TEntity> where TEntity : class
    {
        FrozenDictionary<SortType, Func<TEntity, object>> Functors { get; }
        IQueryable<TEntity> Sort(IQueryable<TEntity> collection, SortType sortType, SortOrder order);
    }
}
