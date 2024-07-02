using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            UserResponse? user = await _authService.GetCurrentUserAsync();

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        [HttpGet("isAuthenticated")]
        public async Task<IActionResult> IsAuthenticatedAsync()
            => Ok(await _authService.IsAuthenticatedAsync());
    }
}
