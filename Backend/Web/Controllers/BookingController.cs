using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.UnitOfWork;
using Entities.Models;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roles;
using Web.ViewModels;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;
        public BookingController(
            ILogger<BookingController> logger,
            IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        [Authorize]
        [HttpPost("book")]
        public async Task<IActionResult> BookEvent(
            [FromBody]BookingViewModel viewModel, CancellationToken cancellationToken = default)
        {
            await _bookingService.BookEventAsync(viewModel, cancellationToken);

            _logger.LogInformation("Event has been booked");

            return Ok();
        }

        [Authorize]
        [HttpDelete("cancel/{id:int}")]
        public async Task<IActionResult> CancelBooking(
            [FromRoute]int id, [FromBody]string userId, CancellationToken cancellationToken = default)
        {
            await _bookingService.CancelBookingAsync(id, userId, cancellationToken);

            _logger.LogInformation("Booking was sucessfully cancelled!");
            return Ok();
        }

        [HttpGet("get-by-participant/{userId}")] 
        public async Task<IActionResult> GetParticipantBookings(string userId, CancellationToken cancellationToken = default)
        {
            ICollection<Booking> bookings = await _bookingService.GetParticipantBookingsAsync(userId, cancellationToken);

            _logger.LogInformation($"Bookings have been obtained. User ID: {userId}");

            return Ok(bookings);
        }

        [HttpGet("participants/{eventId}")]
        public async Task<IActionResult> GetParticipantsByEventId(int eventId, CancellationToken cancellationToken = default)
        {
            ICollection<UserBrief> users = await _bookingService.GetEventParticipants(eventId, cancellationToken);

            return Ok(users);
        }
    }
}
