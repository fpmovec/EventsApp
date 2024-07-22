﻿using Domain.UnitOfWork;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roles;
using Web.ViewModels;
using Application.Interfaces;
using Application.ViewModels;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEventService _eventService;

        public EventsController(
            IUnitOfWork unitOfWork,
            ILogger<EventsController> logger,
            IWebHostEnvironment webHostEnvironment,
            IEventService eventService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _eventService = eventService;
        }

        [Authorize(Roles = nameof(Admin))]
        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent(
            [FromForm]EventViewModel eventViewModel, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _eventService.CreateEventAsync(eventViewModel, _webHostEnvironment.WebRootPath, cancellationToken);

            _logger.LogInformation("Event has been sucessfully created");

            return Created();
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetFilteredEvents(
            [FromQuery]FilterOptionsViewModel options, CancellationToken cancellationToken = default)
        {
            FilteredEventsResponse? response = await _eventService.GetFilteredEventsAsync(options, cancellationToken);

            if (response is null)
                return BadRequest();

            _logger.LogInformation("Events were obtained");

            return Ok(response);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetEventInformation(int id, CancellationToken cancellationToken = default)
        {
            EventExtendedModel? extendedEvent = await _eventService.GetEventInfoAsync(id, cancellationToken);

            if (extendedEvent is null)
            {
                _logger.LogError("Event doesn't exist");
                return NotFound();
            }

            _logger.LogInformation("Extended event was obtained");

            return Ok(extendedEvent);
        }

        [HttpGet("participants/{eventId}")]
        public async Task<IActionResult> GetParticipantsByEventId(int eventId, CancellationToken cancellationToken = default)
        {
            ICollection<UserBrief> users = await _unitOfWork.BookingRepository.GetEventParticipants(eventId);

            return Ok(users);
        }

        [Authorize(Roles = nameof(Admin))]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditEvent(
            [FromRoute]int id,
            [FromForm]EventViewModel eventViewModel,
            CancellationToken cancellationToken = default)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogError("Event model isn't valid!");
                return BadRequest();
            }

            EventExtendedModel extendedEvent =
                await _eventService.EditEventAsync(id, eventViewModel, _webHostEnvironment.WebRootPath, cancellationToken);

            _logger.LogInformation("Event was sucessfully updated");

            return Ok(extendedEvent);
        }

        [Authorize(Roles = nameof(Admin))]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEventAsync(int id, CancellationToken cancellationToken = default)
        {
            await _eventService.DeleteEventByIdAsync(id, cancellationToken);

            _logger.LogInformation("Event ws deleted");

            return Ok();
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetMostPopular(CancellationToken cancellationToken = default)
        {
            ICollection<EventBaseModel> events = await _eventService.GetMostPopularAsync(cancellationToken);

            return Ok(events);
        }
    }
}
