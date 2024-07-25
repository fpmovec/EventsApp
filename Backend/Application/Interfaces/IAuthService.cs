using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Web.DTO;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<AuthTokens> RegisterUserAsync(RegisterDTO registerViewModel, CancellationToken cancellationToken);
        Task<AuthTokens> LoginUserAsync(LoginDTO loginViewModel, CancellationToken cancellationToken);
        Task<AuthTokens> RefreshTokenAsync(TokenRequest tokenRequest, CancellationToken cancellationToken);
        Task LogoutAsync(CancellationToken cancellationToken);
        Task<bool> IsAuthenticatedAsync();
        Task<UserResponse> GetCurrentUserAsync();
        Task<UserResponse> GetUserByIdAsync(string id);
    }
}
