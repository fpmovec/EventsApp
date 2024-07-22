﻿using Application.Interfaces;
using Domain.UnitOfWork;
using Entities.Enums;
using Entities.Exceptions;
using Entities.Models;

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
                throw new ArgumentNullException("name");
            }

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(name, cancellationToken);

            if (category is not null)
            {

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

            }

            await _unitOfWork.CategoryRepository.DeleteByIdAsync(id, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task DeleteCategoryByNameAsync(string name, CancellationToken cancellationToken)
        {
            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(name, cancellationToken);

            if (category is null)
            {
                throw new NotFoundException(ExceptionSubject.Category);
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
