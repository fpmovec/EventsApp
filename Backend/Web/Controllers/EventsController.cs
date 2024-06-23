using Application.Models;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventsController(IUnitOfWork unitOfWork, ILogger<EventsController> logger, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent(
            [FromForm]EventViewModel eventViewModel,
            IFormFile? image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            EventExtendedModel mappedModel = _mapper.Map<EventExtendedModel>(eventViewModel);

            if (image is not null)
            {
                ImageInfo imageInfo = await image.AddImageAsync(_webHostEnvironment.WebRootPath);

                mappedModel.Image = new() { Name = imageInfo.Name, Path = imageInfo.Path };
            }
            else
            {
                mappedModel.Image = new()
                {
                    Name = "no-image",
                    Path = Path.Combine(_webHostEnvironment.WebRootPath, "images", "no-image.jpg")
                };
            }

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByName(eventViewModel.CategoryName);

            if (category is null)
                return BadRequest($"Category {eventViewModel.CategoryName} doesn't exist!");

            mappedModel.Category = category;

            await _unitOfWork.EventsRepository.AddAsync(mappedModel);
            await _unitOfWork.CompleteAsync();

            return Ok(mappedModel);
        }

        [HttpGet("get-all")]
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

        [HttpGet("get/{id:Guid}")]
        public async Task<IActionResult> GetEventInformation(Guid id)
        {
            EventExtendedModel? extendedEvent = await _unitOfWork.EventsRepository.GetExtendedEventByIdAsync(id);

            if (extendedEvent is null)
            {
                _logger.LogError("Event doesn't exist");
                return NotFound();
            }

            _logger.LogInformation("Extended event was obtained");

            return Ok(extendedEvent);
        }

        [HttpGet("participant/{participantId:Guid}")]
        public async Task<IActionResult> GetParticipantEvents(Guid participantId)
        {
            ICollection<EventBaseModel> events = await _unitOfWork.EventsRepository.GetEventsByParticipantIdAsync(participantId);

            return Ok(events);
        }

        [HttpPut("edit/{id:Guid}")]
        public async Task<IActionResult> EditEvent(
            [FromRoute]Guid id,
            [FromForm]EventViewModel eventViewModel,
            IFormFile? image)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogError("Event model isn't valid!");
                return BadRequest();
            }

            EventExtendedModel? extendedEvent = await _unitOfWork.EventsRepository.GetExtendedEventByIdAsync(id);

            if (extendedEvent is null)
            {
                _logger.LogError($"Event with id {id} does not exist!");
                return NotFound();
            }

            Image prevImage = extendedEvent.Image;

            extendedEvent = _mapper.Map(eventViewModel, extendedEvent);

            if (image is not null)
            {
                ImageInfo imageInfo = await image.AddImageAsync(_webHostEnvironment.WebRootPath);

                extendedEvent.Image = new() { Name = imageInfo.Name, Path = imageInfo.Path };
            }
            else
            {
                extendedEvent.Image = prevImage;
            }

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByName(eventViewModel.CategoryName);

            if (category is null)
                return BadRequest($"Category {eventViewModel.CategoryName} doesn't exist!");

            extendedEvent.Category = category;

            await _unitOfWork.EventsRepository.UpdateAsync(extendedEvent);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Event was sucessfully updated");

            return Ok(extendedEvent);
        }

        [HttpDelete("delete/{id:Guid}")]
        public async Task<IActionResult> DeleteEventAsync(Guid id)
        {
            EventBaseModel? eventBase = await _unitOfWork.EventsRepository.GetByIdAsync(id);

            if (eventBase is null)
            {
                _logger.LogError($"Event with the following id({id}) does not exist");
                return NotFound();
            }

            await _unitOfWork.EventsRepository.DeleteByIdAsync(id);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetMostPopular()
        {
            var events =  await _unitOfWork.EventsRepository.GetMostPopularAsync();

            return Ok(events);
        }
    }
}
