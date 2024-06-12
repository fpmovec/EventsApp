using Domain.Enums;

namespace Application.Generic
{
    public interface IGenericRepository<TEntity, TId> where TEntity : class
    {
        Task<ICollection<TEntity>> GetAllAsync(EventFilterType filterType, object filterValue, EventsSortType sortType, SortOrder order, int currentPage);

        Task<TEntity?> GetByIdAsync(TId id);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteByIdAsync(TId id);
    }
}
