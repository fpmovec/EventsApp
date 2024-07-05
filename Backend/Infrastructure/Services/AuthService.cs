using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthService(IHttpContextAccessor contextAccessor, UserManager<IdentityUser> userManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public Task<UserResponse> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = _contextAccessor.HttpContext.User;

            if (currentUser is null || !currentUser.Identity.IsAuthenticated)
                return null;

            string? email = currentUser.FindFirstValue(ClaimTypes.Email);
            string? name = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            string id = currentUser.Claims.FirstOrDefault(c => c.Type == "Id")!.Value;
            string? phone = currentUser.Claims.FirstOrDefault(c => c.Type == "Phone")?.Value;

            return Task.FromResult(new UserResponse()
            {
                Email = email,
                Id = id,
                Name = name,
                Phone = phone ?? ""
            });
        }

        public async Task<UserResponse?> GetUserByIdAsync(string id)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(id);

            if (user is null) 
                return null;

            return new()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber
            };
        }

        public async Task<bool> IsAuthenticatedAsync()
            => await Task.FromResult(_contextAccessor.HttpContext.User.Identity.IsAuthenticated);
    }
}
