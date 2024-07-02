using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticatedAsync();
        Task<UserResponse> GetCurrentUserAsync();
    }
}
