using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Web.ViewModels;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<AuthTokens> RegisterUserAsync(RegisterViewModel registerViewModel, CancellationToken cancellationToken);
        Task<AuthTokens> LoginUserAsync(LoginViewModel loginViewModel, CancellationToken cancellationToken);
        Task<AuthTokens> RefreshTokenAsync(TokenRequest tokenRequest, CancellationToken cancellationToken);
        Task LogoutAsync(CancellationToken cancellationToken);
        Task<bool> IsAuthenticatedAsync();
        Task<UserResponse> GetCurrentUserAsync();
        Task<UserResponse?> GetUserByIdAsync(string id);
    }
}
