using Application.Services;
using AutoMapper;
using Domain.UnitOfWork;
using Entities.Models;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingController> _logger;

        public BookingController(
            IUnitOfWork unitOfWork,
            ILogger<BookingController> logger,
            IAuthService authService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _authService = authService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("book")]
        public async Task<IActionResult> BookEvent([FromBody]BookingViewModel viewModel, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Booking model is not valid!");
                return BadRequest(ModelState);
            }

            var bookedEvent = await _unitOfWork.EventsRepository.GetByIdAsync(viewModel.EventId);

            if (bookedEvent is null)
            {
                _logger.LogError($"The event with id {viewModel.EventId} does not exist!");
                return BadRequest(ModelState);
            }

            Booking booking = new()
            {
                EventName = bookedEvent.Name,
                PricePerOne = bookedEvent.Price
            };

            booking = _mapper.Map(viewModel, booking);

            await _unitOfWork.BookingRepository.BookEventAsync(booking);
            await _unitOfWork.EventsRepository.BookTickets(bookedEvent.Id, booking.PersonsQuantity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Event has been booked");

            return Ok();
        }

        [Authorize]
        [HttpDelete("cancel/{id:int}")]
        public async Task<IActionResult> CancelBooking([FromRoute]int id, [FromBody]string userId, CancellationToken cancellationToken = default)
        {
            Booking? booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);

            if (booking is null)
            {
                _logger.LogError("Booking not found");
                return NotFound();
            }

            if (booking.UserId != userId)
            {
                _logger.LogError("You can't delete this booking!");
                return BadRequest();
            }

            await _unitOfWork.BookingRepository.CancelBooking(booking.Id);
            await _unitOfWork.EventsRepository.CancelTickets(booking.EventId, booking.PersonsQuantity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Booking was sucessfully cancelled!");
            return Ok();
        }

        [HttpGet("get-by-participant/{userId}")] 
        public async Task<IActionResult> GetParticipantBookings(string userId)
        {
            ICollection<Booking> bookings = await _unitOfWork.BookingRepository.GetParticipantBookingsAsync(userId);

            _logger.LogInformation($"Bookings have been obtained. User ID: {userId}");

            return Ok(bookings);
        }
    }
}
