using Application.Generic;
using Domain.Models;

namespace Application.Repositories
{
    public interface IParticipantRepository : IGenericRepository<Participant, int>
    {
        Task<Participant?> GetParticipantByemailAsync(string email);
    }
}
