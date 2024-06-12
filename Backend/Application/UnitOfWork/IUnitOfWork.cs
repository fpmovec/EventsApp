using Application.Repositories;

namespace Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        IEventsRepository EventsRepository { get; }
        Task CompleteAsync();
    }
}
