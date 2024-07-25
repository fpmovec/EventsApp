using Application.Interfaces;
using AutoMapper;
using Domain.UnitOfWork;
using Domain.Exceptions;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Web.ViewModels;
using Domain.Exceptions.ExceptionMessages;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly IValidator<BookingViewModel> _validator;

        public BookingService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IEventService eventService,
            IValidator<BookingViewModel> validator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _eventService = eventService;
            _validator = validator;
        }

        public async Task BookEventAsync(BookingViewModel viewModel, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(viewModel, cancellationToken);

            if (!validationResult.IsValid)
                throw new BadRequestException("BookingViewModel is invalid");

            var bookedEvent = await _unitOfWork.EventsRepository.GetByIdAsync(viewModel.EventId, cancellationToken);

            if (bookedEvent is null)
            {
                throw new NotFoundException(NotFoundExceptionMessages.EventNotFound);
            }

            Booking booking = new()
            {
                EventName = bookedEvent.Name,
                PricePerOne = bookedEvent.Price
            };

            booking = _mapper.Map(viewModel, booking);

            await _unitOfWork.BookingRepository.BookEventAsync(booking, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _eventService.BookTickets(bookedEvent.Id, booking.PersonsQuantity, cancellationToken);
        }

        public async Task CancelBookingAsync(int id, string userId, CancellationToken cancellationToken)
        {
            Booking? booking = await _unitOfWork.BookingRepository.GetByIdAsync(id, cancellationToken);

            if (booking is null)
            {
                throw new NotFoundException(NotFoundExceptionMessages.BookingNotFound);
            }

            if (booking.UserId != userId)
            {
                throw new BadRequestException("You can't delete this booking!");
            }

            await _unitOfWork.BookingRepository.CancelBookingAsync(booking.Id, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _eventService.CancelTickets(booking.EventId, booking.PersonsQuantity, cancellationToken);
        }

        public async Task<ICollection<UserBrief>> GetEventParticipants(int eventId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.BookingRepository.GetEventParticipantsAsync(eventId, cancellationToken);
        }

        public async Task<ICollection<Booking>> GetParticipantBookingsAsync(string userId, CancellationToken cancellationToken)
        {
            ICollection<Booking> bookings = await _unitOfWork.BookingRepository.GetParticipantBookingsAsync(userId, cancellationToken);

            return bookings;
        }
    }
}
