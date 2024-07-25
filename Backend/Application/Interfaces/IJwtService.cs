using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IJwtService
    {
        public Task<AuthTokens> GenerateJwtTokensAsync(IdentityUser user, CancellationToken cancellationToken);

        public Task<AuthTokens?> VerifyAndGenerateTokenAsync(AuthTokens tokens, UserManager<IdentityUser> userManager, CancellationToken cancellationToken);

        public Task DeleteUserRefreshTokensAsync(string uderId, CancellationToken cancellationToken);

        public Task ClearUnusedRefreshTokensAsync(CancellationToken cancellationToken);
    }
}
