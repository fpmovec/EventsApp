using Application.Interfaces;
using Domain.UnitOfWork;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.Exceptions.ExceptionMessages;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EventCategory> CreateCategoryAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new NullObjectException("Name cannot be null!");
            }

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(name, cancellationToken);

            if (category is not null)
            {
                throw new AlreadyExistsException(AlreadyExistsExceptionMessages.CategoryAlreadyExists);
            }

            category = new() { Name = name };

            await _unitOfWork.CategoryRepository.AddAsync(category, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return category;
        }

        public async Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            EventCategory? category = await _unitOfWork.CategoryRepository.GetByIdAsync(id, cancellationToken);

            if (category is null)
            {
                throw new NullObjectException(NullObjectExceptionMessages.NullCategory);
            }

            await _unitOfWork.CategoryRepository.DeleteByIdAsync(id, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task DeleteCategoryByNameAsync(string name, CancellationToken cancellationToken)
        {
            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(name, cancellationToken);

            if (category is null)
            {
                throw new NotFoundException(NotFoundExceptionMessages.CategoryNotFound);
            }

            await _unitOfWork.CategoryRepository.DeleteByIdAsync(category.Id, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task<ICollection<string>> GetCategoriesNamesAsync(CancellationToken cancellationToken)
        {
            IQueryable<EventCategory> categories = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();

            return categories.Select(c => c.Name).ToList();
        }
    }
}
