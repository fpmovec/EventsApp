﻿using Domain.Repositories;

namespace Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        IEventsRepository EventsRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IBookingRepository BookingRepository { get; }
        IJwtRepository JwtRepository { get; }
        Task CompleteAsync(CancellationToken cancellationToken);
    }
}
