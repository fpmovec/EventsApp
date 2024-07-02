using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Task<UserResponse> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = _contextAccessor.HttpContext.User;

            if (currentUser is null || !currentUser.Identity.IsAuthenticated)
                return null;

            string? email = currentUser.FindFirstValue(ClaimTypes.Email);
            string? name = currentUser.FindFirstValue(ClaimTypes.Name);
            string id = currentUser.Claims.FirstOrDefault(c => c.Type == "Id")!.Value;

            return Task.FromResult(new UserResponse()
            {
                Email = email,
                Id = id,
                Name = name,
            });
        }

        public async Task<bool> IsAuthenticatedAsync()
            => await Task.FromResult(_contextAccessor.HttpContext.User.Identity.IsAuthenticated);
    }
}
