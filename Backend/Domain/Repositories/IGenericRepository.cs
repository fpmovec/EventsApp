using Entities.Enums;
using Entities.Models;

namespace Domain.Repositories
{
    public interface IGenericRepository<TEntity, TId> where TEntity : class
    {
        Task<(IQueryable<TEntity>, int)> GetAllAsync(List<FilterOption> filterOptions, SortType sortType, SortOrder order, int currentPage);

        Task<TEntity?> GetByIdAsync(TId id);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteByIdAsync(TId id);
    }
}
