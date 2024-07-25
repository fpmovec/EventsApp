using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;
using Web.DTO;

namespace Web.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthService _authService;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [AnonymousOnly]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(
            [FromBody] RegisterDTO registerViewModel, CancellationToken cancellationToken = default)
        {
            AuthTokens tokens = await _authService.RegisterUserAsync(registerViewModel, cancellationToken);

            return Ok(tokens);
        }

        [AnonymousOnly]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDTO loginViewModel, CancellationToken cancellationToken = default)
        {
            AuthTokens tokens = await _authService.LoginUserAsync(loginViewModel, cancellationToken);

            return Ok(tokens);
        }

        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] TokenRequest tokenRequest, CancellationToken cancellationToken = default)
        {
            AuthTokens tokens = await _authService.RefreshTokenAsync(tokenRequest, cancellationToken);

            return Ok(tokens);
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> LogOut(CancellationToken cancellationToken = default)
        {
            await _authService.LogoutAsync(cancellationToken);

            _logger.LogInformation("You logged out");

            return Ok();
        }
    }
}
