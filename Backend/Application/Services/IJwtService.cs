using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IJwtService
    {
        public string GenerateJwtToken(IdentityUser user);
    }
}
