using Application.Services;
using Domain.AppSettings;
using Domain.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly EventContext _eventContext;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtService(
            IOptions<AppSettings> options,
            EventContext eventContext,
            TokenValidationParameters tokenValidationParameters)
        {
            _jwtSettings = options.Value.JwtSettings;
            _eventContext = eventContext;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthTokens> GenerateJwtTokens(IdentityUser user)
        {
           var jwtHandler = new JwtSecurityTokenHandler();
           byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.ExpirationTimeFrame),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            SecurityToken token = jwtHandler.CreateToken(tokenDescriptor);
            string jwtToken = jwtHandler.WriteToken(token);
            string refreshToken = await GetAndSaveRefreshToken(token.Id, user.Id);

            return new() { MainToken = jwtToken, RefreshToken = refreshToken };
        }

        public async Task<AuthTokens?> VerifyAndGenerateToken(AuthTokens tokenRequest, UserManager<IdentityUser> userManager)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            try
            {
                _tokenValidationParameters.ValidateLifetime = false; // for testing

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

                RefreshToken? storedRefreshToken = await _eventContext.RefreshTokens
                    .FirstOrDefaultAsync(t => t.Token == tokenRequest.RefreshToken);

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

                _eventContext.RefreshTokens.Update(storedRefreshToken);
                await _eventContext.SaveChangesAsync();

                IdentityUser? user = await userManager.FindByIdAsync(storedRefreshToken.UserId);

                if (user is null)
                {
                    return null;
                }

                return await GenerateJwtTokens(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> GetAndSaveRefreshToken(string id, string userId)
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

            await _eventContext.RefreshTokens.AddAsync(refreshToken);
            await _eventContext.SaveChangesAsync();

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
