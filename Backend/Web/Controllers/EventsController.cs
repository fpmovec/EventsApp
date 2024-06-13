using Application.Models;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.ViewModels;

namespace Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EventsController(IUnitOfWork unitOfWork, ILogger<EventsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent(
            [FromForm]EventViewModel eventViewModel,
            IFormFile? image,
            [FromServices]IWebHostEnvironment environment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            EventExtendedModel mappedModel = _mapper.Map<EventExtendedModel>(eventViewModel);

            if (image is not null)
            {
                ImageInfo imageInfo = await image.AddImageAsync(environment.WebRootPath);

                mappedModel.Image = new() { Name = imageInfo.Name, Path = imageInfo.Path };
            }

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByName(eventViewModel.CategoryName);

            if (category is null)
                return BadRequest($"Category {eventViewModel.CategoryName} doesn't exist!");

            mappedModel.Category = category;

            await _unitOfWork.EventsRepository.AddAsync(mappedModel);
            await _unitOfWork.CompleteAsync();

            return Ok(mappedModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredEvents([FromQuery]FilterOptionsViewModel options)
        {
            List<FilterOption> filterOptions = new();

            if (options is not null)
                filterOptions = _mapper.Map<List<FilterOption>>(options);
            else
                return BadRequest();

            var events = await _unitOfWork.EventsRepository.GetFilteredEventsAsync(
                filterOptions,
                options.SortType, options.SortOrder, options.CurrentPage);

            _logger.LogInformation("Events were obtained");

            return Ok(events);
        }
    }
}
