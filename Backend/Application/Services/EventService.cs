using Application.Extensions;
using Application.Interfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Repositories;
using Domain.UnitOfWork;
using Domain.Exceptions;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Web.DTO;
using Domain.Exceptions.ExceptionMessages;

namespace Application.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IValidator<EventDTO> _validator;

        public EventService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            INotificationService notificationService,
            IValidator<EventDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _validator = validator;
        }

        public async Task BookTickets(int eventId, int bookedTickets, CancellationToken cancellationToken)
        {
            EventExtendedModel? bookedEvent = await _unitOfWork.EventsRepository.GetExtendedEventByIdAsync(eventId, cancellationToken);

            if (bookedEvent is null)
                throw new NotFoundException(NotFoundExceptionMessages.EventNotFound);

            bookedEvent.BookedTicketsCount =+ bookedTickets;

            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task CancelTickets(int eventId, int bookedTickets, CancellationToken cancellationToken)
        {
            EventExtendedModel? bookedEvent = await _unitOfWork.EventsRepository.GetExtendedEventByIdAsync(eventId, cancellationToken);

            if (bookedEvent is null)
                throw new NotFoundException(NotFoundExceptionMessages.EventNotFound);

            bookedEvent.BookedTicketsCount =- bookedTickets;

            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task CreateEventAsync(
            EventDTO eventViewModel,
            string webRootPath,
            CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(eventViewModel, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("EventViewModel is invalid");
            }

            EventExtendedModel mappedModel = _mapper.Map<EventExtendedModel>(eventViewModel);

            if (eventViewModel.ImageFile is not null)
            {
                ImageInfo imageInfo = await eventViewModel.ImageFile.AddImageAsync(webRootPath, cancellationToken);

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

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(eventViewModel.CategoryName, cancellationToken);

            if (category is null)
            {
                category = new() { Name = eventViewModel.CategoryName };
                await _unitOfWork.CategoryRepository.AddAsync(category, cancellationToken);
            }

            mappedModel.Category = category;

            await _unitOfWork.EventsRepository.AddAsync(mappedModel, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task DeleteEventByIdAsync(int id, CancellationToken cancellationToken)
        {
            EventBaseModel? eventBase = await _unitOfWork.EventsRepository.GetByIdAsync(id, cancellationToken);

            if (eventBase is null)
            {
                throw new NotFoundException(NotFoundExceptionMessages.EventNotFound);
            }

            await _unitOfWork.EventsRepository.DeleteByIdAsync(id, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task<EventExtendedModel> EditEventAsync(
            int id, EventDTO eventViewModel, string webRootPath, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(eventViewModel, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("EventViewModel is invalid");
            }

            EventExtendedModel? extendedEvent = await _unitOfWork.EventsRepository.GetExtendedEventByIdAsync(id, cancellationToken);

            if (extendedEvent is null)
            {
                throw new NotFoundException(NotFoundExceptionMessages.EventNotFound);
            }

            string oldName = extendedEvent.Name;

            Image prevImage = extendedEvent.Image;

            extendedEvent = _mapper.Map(eventViewModel, extendedEvent);

            if (eventViewModel.ImageFile is not null)
            {
                ImageInfo imageInfo = await eventViewModel.ImageFile.AddImageAsync(webRootPath, cancellationToken);

                extendedEvent.Image = new() { Name = imageInfo.Name, Path = imageInfo.Path };
            }
            else
            {
                extendedEvent.Image = prevImage;
            }

            EventCategory? category = await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(eventViewModel.CategoryName, cancellationToken);

            if (category is null)
            {
                category = new() { Name = eventViewModel.CategoryName };
                await _unitOfWork.CategoryRepository.AddAsync(category, cancellationToken);
            }

            extendedEvent.Category = category;

            var particiapnts = await _unitOfWork.BookingRepository.GetEventParticipantsAsync(extendedEvent.Id, cancellationToken);

            await _unitOfWork.EventsRepository.UpdateAsync(extendedEvent);
            await _notificationService.NotifyUsersAsync(oldName, extendedEvent.Id, particiapnts, cancellationToken);
            await _notificationService.NotifyCurrentUserWithPopupAsync(oldName, extendedEvent.Id, particiapnts);
            await _unitOfWork.BookingRepository.UpdateDependingBookingsAsync(extendedEvent);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return extendedEvent;
        }

        public async Task<EventExtendedModel> GetEventInfoAsync(int id, CancellationToken cancellationToken)
        {
            EventExtendedModel? eventExtended =  await _unitOfWork.EventsRepository.GetExtendedEventByIdAsync(id, cancellationToken);

            if (eventExtended is null)
            {
                throw new NotFoundException(NotFoundExceptionMessages.EventNotFound);
            }

            return eventExtended;
        }

        public async Task<FilteredEventsResponse> GetFilteredEventsAsync(
            FilterOptionsDTO options, CancellationToken cancellationToken)
        {
            List<FilterOption> filterOptions = [];

            if (options is null)
                throw new BadRequestException("Options are invalid!");

            filterOptions = _mapper.Map<List<FilterOption>>(options);

            (ICollection<EventBaseModel> events, int pages) = await _unitOfWork.EventsRepository.GetFilteredEventsAsync(
                filterOptions,
                options.SortType, options.SortOrder, options.CurrentPage);

            return new() { Events = events, Pages = pages };
        }

        public async Task<ICollection<EventBaseModel>> GetMostPopularAsync(CancellationToken cancellationToken)
        {
           return await _unitOfWork.EventsRepository.GetMostPopularAsync(cancellationToken);
        }
    }
}
