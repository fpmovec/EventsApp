using Application.CollectionServices;
using Domain.AppSettings;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class JwtRepository : GenericRepository<RefreshToken, int>, IJwtRepository
    {
        public JwtRepository(
            EventContext eventContext,
            ILogger<JwtRepository> logger,
            IOptions<AppSettings> options,
            IFilterService<RefreshToken> filterService,
            ISortService<RefreshToken> sortService
            ) : base(eventContext, logger, options, filterService, sortService) { }

        protected override DbSet<RefreshToken> dbSet => _eventContext.RefreshTokens;
        public async Task DeleteExpiredRefreshTokensAsync(CancellationToken cancellationToken)
        {
            var tokensForRemoving = await _eventContext.RefreshTokens
                    .Where(r => r.IsUsed || (r.ExpiryDate < DateTime.UtcNow)).ToListAsync();

            _eventContext.RemoveRange(tokensForRemoving);
        }

        public async Task DeleteRefreshTokensByUserId(string userId, CancellationToken cancellationToken)
        {
            var token = _eventContext.RefreshTokens.Where(r => r.UserId.Equals(userId));

            _eventContext.Remove(token);

            await Task.CompletedTask;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            var refreshToken = await _eventContext.RefreshTokens
                .Where(t => t.Token == token)
                .FirstOrDefaultAsync(cancellationToken);

            return refreshToken;
        }

        public async Task<bool> IsRefreshTokensExists()
        {
            bool isDbSetEmpty = dbSet.Any();

            return await Task.FromResult(isDbSetEmpty);
        }
    }
}
