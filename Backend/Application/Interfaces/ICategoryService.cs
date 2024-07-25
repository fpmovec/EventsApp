using Domain.Models;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<EventCategory> CreateCategoryAsync(string name, CancellationToken cancellationToken);

        Task<ICollection<string>> GetCategoriesNamesAsync(CancellationToken cancellationToken);

        Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken);

        Task DeleteCategoryByNameAsync(string name, CancellationToken cancellationToken);
    }
}
