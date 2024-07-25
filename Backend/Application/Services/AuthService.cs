using Domain.Exceptions;
using Domain.Exceptions;
using Domain.Exceptions.ExceptionMessages;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Roles;
using System.Security.Claims;
using Web.DTO;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IValidator<LoginDTO> _loginValidator;
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly IValidator<TokenRequest> _tokensValidator;

        public AuthService(
            IHttpContextAccessor contextAccessor,
            UserManager<IdentityUser> userManager,
            IJwtService jwtService,
            IValidator<LoginDTO> loginValidator,
            IValidator<RegisterDTO> registerValidator,
            IValidator<TokenRequest> tokensValidator)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _jwtService = jwtService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _tokensValidator = tokensValidator;
        }

        public async Task<UserResponse> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = _contextAccessor.HttpContext.User;

            if (currentUser is null || !currentUser.Identity.IsAuthenticated)
                throw new NotAuthenticatedException();

            string? email = currentUser.FindFirstValue(ClaimTypes.Email);
            string role = currentUser.FindFirstValue(ClaimTypes.Role);
            string? name = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            string id = currentUser.Claims.FirstOrDefault(c => c.Type == "Id")!.Value;
            string? phone = currentUser.Claims.FirstOrDefault(c => c.Type == "Phone")?.Value;

            return await Task.FromResult(new UserResponse()
            {
                Email = email,
                Id = id,
                Name = name,
                Phone = phone ?? "",
                IsAdmin = role.Equals(nameof(Admin))
            });
        }

        public async Task<UserResponse> GetUserByIdAsync(string id)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(id);

            if (user is null)
                throw new NotFoundException(NotFoundExceptionMessages.UserNotFound);

            return new()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
            };
        }

        public async Task<bool> IsAuthenticatedAsync()
            => await Task.FromResult(_contextAccessor.HttpContext.User.Identity.IsAuthenticated);

        public async Task<AuthTokens> LoginUserAsync(LoginDTO loginViewModel, CancellationToken cancellationToken)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginViewModel, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("LoginDTO is invalid!");
            }

            IdentityUser? user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user is null)
            {
                throw new NotFoundException(NotFoundExceptionMessages.UserNotFound);
            }

            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            if (!isPasswordCorrect)
            {
                throw new BadRequestException(BadRequestExceptionMessages.InvalidPassword);
            }

            AuthTokens tokens = await _jwtService.GenerateJwtTokensAsync(user, cancellationToken);

            return tokens;
        }

        public async Task LogoutAsync(CancellationToken cancellationToken)
        {
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new NotAuthenticatedException();
            }

            string currentUserId = _contextAccessor.HttpContext.User.Identities.First().Claims.FirstOrDefault(c => c.Type == "Id").Value;

            IdentityUser? currentUser = await _userManager.FindByIdAsync(currentUserId);

            if (currentUser is not null)
            {
                await _jwtService.DeleteUserRefreshTokensAsync(currentUser.Id, cancellationToken);
            }
        }

        public async Task<AuthTokens> RefreshTokenAsync(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            var validationResult = await _tokensValidator.ValidateAsync(tokenRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Tokens are invalid!");
            }

            AuthTokens? tokens = await _jwtService.VerifyAndGenerateTokenAsync(new()
            {
                MainToken = tokenRequest.MainToken,
                RefreshToken = tokenRequest.RefreshToken
            }, _userManager, cancellationToken);

            if (tokens is null)
            {
                throw new BadRequestException(BadRequestExceptionMessages.ExpiredSession);
            }
            else return tokens;
        }

        public async Task<AuthTokens> RegisterUserAsync(RegisterDTO registerViewModel, CancellationToken cancellationToken)
        {
            var validationResult = await _registerValidator.ValidateAsync(registerViewModel, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("RegisterDTO is invalid!");
            }

            IdentityUser? user = await _userManager.FindByEmailAsync(registerViewModel.Email);

            if (user is not null)
            {
                throw new AlreadyExistsException(AlreadyExistsExceptionMessages.UserAlreadyExists);
            }

            IdentityUser newUser = new() { Email = registerViewModel.Email, UserName = registerViewModel.Name, PhoneNumber = registerViewModel.Phone };

            IdentityResult isSucessfullyCreated = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (isSucessfullyCreated.Succeeded)
            {
                AuthTokens tokens = await _jwtService.GenerateJwtTokensAsync(newUser, cancellationToken);

                return tokens;
            }

            throw new UnsucessfulOperationResultException();
        }
    }
}
