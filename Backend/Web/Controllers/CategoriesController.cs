using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork, ILogger<EventsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategoryAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger.LogError("Error. Category name is empty!");
                return BadRequest("Category name cannot be empty");
            }
                

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByName(name);

            if (category is not null)
            {
                _logger.LogError($"Error. Category {name} is already exist!");
                return BadRequest("Category is already exist");
            }
                

            category = new()
            {
                Name = name
            };

            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Category was sucessfully added");

            return Ok(category);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            _logger.LogInformation("All categories were obtained");
            var categories = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteCategoryByIdAsync(int id)
        {
            EventCategory? category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                _logger.LogError("Error. Category with the following id is missing");
                return NotFound("Category with the following id is missing");
            }

            await _unitOfWork.CategoryRepository.DeleteByIdAsync(id);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Category {category.Name} deleted");

            return Ok(category.Name);
        }

        [HttpDelete("deleteByName/{name}")]
        public async Task<IActionResult> DeleteCategoryByNameAsync(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger.LogError("Category ame cannot be empty");
                return BadRequest();
            }

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByName(name);

            if (category is null)
            {
                _logger.LogError("Error. Category with the following name is missing");
                return NotFound("Category with the following name is missing");
            }

            await _unitOfWork.CategoryRepository.DeleteByIdAsync(category.Id);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Category {category.Name} was deleted");

            return Ok(category.Name);
        }
    }
}
