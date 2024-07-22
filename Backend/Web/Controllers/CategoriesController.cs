using Application.Interfaces;
using Domain.UnitOfWork;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roles;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ILogger<EventsController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [Authorize(Roles = nameof(Admin))]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategoryAsync(string name, CancellationToken cancellationToken = default)
        {
            EventCategory category = await _categoryService.CreateCategoryAsync(name, cancellationToken);

            _logger.LogInformation("Category was sucessfully added");

            return Ok(category);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _categoryService.GetCategoriesNamesAsync(cancellationToken);

            _logger.LogInformation($"Categories were obtained");

            return Ok(categories);
        }

        [Authorize(Roles = nameof(Admin))]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await  _categoryService.DeleteCategoryByIdAsync(id, cancellationToken);

            _logger.LogInformation($"Category was deleted");

            return Ok();
        }

        [Authorize(Roles = nameof(Admin))]
        [HttpDelete("deleteByName/{name}")]
        public async Task<IActionResult> DeleteCategoryByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            await _categoryService.DeleteCategoryByNameAsync(name, cancellationToken);

            _logger.LogInformation($"Category was deleted");

            return Ok();
        }
    }
}
