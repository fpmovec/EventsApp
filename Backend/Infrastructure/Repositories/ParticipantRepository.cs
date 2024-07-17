using Application.CollectionServices;
using Application.Repositories;
using Domain.AppSettings;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class ParticipantRepository : GenericRepository<Participant, int>, IParticipantRepository
    {
        public ParticipantRepository(
            EventContext eventContext,
            Logger<ParticipantRepository> logger,
            IOptions<AppSettings> options,
            IFilterService<Participant> filterService,
            ISortService<Participant> sortService
            ) : base(eventContext, logger, options, filterService, sortService) { }

        protected override DbSet<Participant> dbSet => _eventContext.Participants;

        public async Task<Participant?> GetParticipantByemailAsync(string email)
        {
            return await dbSet.Where(p => p.Email == email).FirstOrDefaultAsync();
        }
    }
}
