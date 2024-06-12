using Application.Generic;
using Domain.Models;

namespace Application.Repositories
{
    public interface IEventsRepository : IGenericRepository<EventBaseModel, Guid>
    {
    }
}
