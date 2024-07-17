using Application.Models;
using Application.Services;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roles;
using Web.Extensions;
using Web.ViewModels;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotificationService _notificationService;

        public EventsController(
            IUnitOfWork unitOfWork,
            ILogger<EventsController> logger,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _notificationService = notificationService;
        }

        [Authorize(Roles = nameof(Admin))]
        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent(
            [FromForm]EventViewModel eventViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            EventExtendedModel mappedModel = _mapper.Map<EventExtendedModel>(eventViewModel);

            if (eventViewModel.ImageFile is not null)
            {
                ImageInfo imageInfo = await eventViewModel.ImageFile.AddImageAsync(_webHostEnvironment.WebRootPath);

                mappedModel.Image = new() { Name = imageInfo.Name, Path = imageInfo.Path };
            }
            else
            {
                mappedModel.Image = new()
                {
                    Name = "no-image",
                    Path = Path.Combine("images", "no-image.jpg")
                };
            }

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByName(eventViewModel.CategoryName);

            if (category is null)
            {
                category = new() { Name = eventViewModel.CategoryName };
                await _unitOfWork.CategoryRepository.AddAsync(category);
            }

            mappedModel.Category = category;

            await _unitOfWork.EventsRepository.AddAsync(mappedModel);
            await _unitOfWork.CompleteAsync();

            return Created();
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

            return Ok(new { Events = events.Item1, pages = events.Item2});
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetEventInformation(int id)
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

        [HttpGet("participants/{eventId}")]
        public async Task<IActionResult> GetParticipantsByEventId(int eventId)
        {
            ICollection<UserBrief> users = await _unitOfWork.BookingRepository.GetEventParticipants(eventId);

            return Ok(users);
        }

        [Authorize(Roles = nameof(Admin))]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditEvent(
            [FromRoute]int id,
            [FromForm]EventViewModel eventViewModel)
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
            string oldName = extendedEvent.Name;

            Image prevImage = extendedEvent.Image;

            extendedEvent = _mapper.Map(eventViewModel, extendedEvent);

            if (eventViewModel.ImageFile is not null)
            {
                ImageInfo imageInfo = await eventViewModel.ImageFile.AddImageAsync(_webHostEnvironment.WebRootPath);

                extendedEvent.Image = new() { Name = imageInfo.Name, Path = imageInfo.Path };
            }
            else
            {
                extendedEvent.Image = prevImage;
            }

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByName(eventViewModel.CategoryName);

            if (category is null)
                await _unitOfWork.CategoryRepository.AddAsync(new() { Name = eventViewModel.CategoryName });

            extendedEvent.Category = category;

            var particiapnts = await _unitOfWork.BookingRepository.GetEventParticipants(extendedEvent.Id);

            await _unitOfWork.EventsRepository.UpdateAsync(extendedEvent);
            await _notificationService.NotifyUsersAsync(oldName, extendedEvent.Id, particiapnts);
            await _notificationService.NotifyCurrentUserWithPopupAsync(oldName, extendedEvent.Id, particiapnts);
            await _unitOfWork.BookingRepository.UpdateDependingBookingsAsync(extendedEvent);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Event was sucessfully updated");

            return Ok(extendedEvent);
        }

        [Authorize(Roles = nameof(Admin))]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEventAsync(int id)
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
