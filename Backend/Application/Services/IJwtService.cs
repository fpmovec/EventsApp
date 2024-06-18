using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IJwtService
    {
        public Task<AuthTokens> GenerateJwtTokens(IdentityUser user);
        public Task<AuthTokens?> VerifyAndGenerateToken(AuthTokens tokens, UserManager<IdentityUser> userManager);
    }
}
