using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IJwtService
    {
        public Task<AuthTokens> GenerateJwtTokensAsync(IdentityUser user);

        public Task<AuthTokens?> VerifyAndGenerateTokenAsync(AuthTokens tokens, UserManager<IdentityUser> userManager);

        public Task DeleteUserRefreshTokensAsync(string uderId);

        public Task ClearUnusedRefreshTokensAsync();
    }
}
