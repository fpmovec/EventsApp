using Domain.Enums;

namespace Application.Generic
{
    public interface IGenericRepository<TEntity, TId> where TEntity : class
    {
        IQueryable<TEntity> GetAllAsync(EventFilterType? filterType, object filterValue, EventsSortType sortType, SortOrder order, int currentPage);

        TEntity GetByIdAsync(TId id);
    }
}
