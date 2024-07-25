using Domain.Models;

namespace Domain.Repositories
{
    public interface IJwtRepository : IGenericRepository<RefreshToken, int>
    {
        Task DeleteExpiredRefreshTokensAsync(CancellationToken cancellationToken);
        Task DeleteRefreshTokensByUserId(string userId, CancellationToken cancellationToken);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
        Task<bool> IsRefreshTokensExists();
    }
}
