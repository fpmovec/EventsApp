using Application.Repositories;

namespace Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        IEventsRepository EventsRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IBookingRepository BookingRepository { get; }
        Task CompleteAsync();
    }
}
