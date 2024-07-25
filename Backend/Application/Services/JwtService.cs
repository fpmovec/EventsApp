using Domain.AppSettings;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Roles;
using Domain.UnitOfWork;
using Application.Extensions;

namespace Application.Services
{
    public class JwtService : IJwtService
    {
        private const string AdminTestEmail = "admin@example.com";

        private readonly JwtSettings _jwtSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtService(
            IOptions<AppSettings> options,
            TokenValidationParameters tokenValidationParameters,
            IUnitOfWork unitOfWork)
        {
            _jwtSettings = options.Value.JwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthTokens> GenerateJwtTokensAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Phone", user.PhoneNumber ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.ExpirationTimeFrame),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            if (user.Email.Equals(AdminTestEmail))
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, nameof(Admin)));
            }
            else
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, nameof(User)));
            }

            SecurityToken token = jwtHandler.CreateToken(tokenDescriptor);
            string jwtToken = jwtHandler.WriteToken(token);
            string refreshToken = await GetAndSaveRefreshToken(token.Id, user.Id, cancellationToken);

            return new() { MainToken = jwtToken, RefreshToken = refreshToken };
        }

        public async Task<AuthTokens?> VerifyAndGenerateTokenAsync(
            AuthTokens tokenRequest, UserManager<IdentityUser> userManager, CancellationToken cancellationToken)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            try
            {
                _tokenValidationParameters.ValidateLifetime = true;

                ClaimsPrincipal? tokenInVerification = tokenHandler.ValidateToken(
                    tokenRequest.MainToken, _tokenValidationParameters, out var securityValidatedToken);

                if (securityValidatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    bool isSHA256 = jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!isSHA256)
                    {
                        return null;
                    }
                }

                long tokenExpiryUtcDate = long.Parse(tokenInVerification.Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

                DateTime expiryDate = tokenExpiryUtcDate.UnixTimeStampToDateTime();

                if (expiryDate > DateTime.Now)
                {
                    return null;
                }

                RefreshToken? storedRefreshToken = await _unitOfWork.JwtRepository
                    .GetByTokenAsync(tokenRequest.RefreshToken, cancellationToken);

                if (storedRefreshToken is null ||
                    storedRefreshToken.IsUsed ||
                    storedRefreshToken.IsRevoked)
                {
                    return null;
                }

                string? jti = tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedRefreshToken.JwtId != jti || storedRefreshToken.ExpiryDate < DateTime.UtcNow)
                {
                    return null;
                }

                storedRefreshToken.IsUsed = true;

                await _unitOfWork.JwtRepository.UpdateAsync(storedRefreshToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                IdentityUser? user = await userManager.FindByIdAsync(storedRefreshToken.UserId);

                if (user is null)
                {
                    return null;
                }

                return await GenerateJwtTokensAsync(user, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteUserRefreshTokensAsync(string userId, CancellationToken cancellationToken)
        {
            await _unitOfWork.JwtRepository.DeleteRefreshTokensByUserId(userId, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task ClearUnusedRefreshTokensAsync(CancellationToken cancellationToken)
        {
            if (await _unitOfWork.JwtRepository.IsRefreshTokensExists())
            {
                await _unitOfWork.JwtRepository.DeleteExpiredRefreshTokensAsync(cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        private async Task<string> GetAndSaveRefreshToken(string id, string userId, CancellationToken cancellationToken)
        {
            RefreshToken refreshToken = new()
            {
                JwtId = id,
                Token = GenerateRandomString(23),
                CreatedAt = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddHours(_jwtSettings.RefreshTokenExpirationInHours),
                IsRevoked = false,
                IsUsed = false,
                UserId = userId
            };

            await _unitOfWork.JwtRepository.AddAsync(refreshToken, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return refreshToken.Token;
        }

        private string GenerateRandomString(int length)
        {
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
