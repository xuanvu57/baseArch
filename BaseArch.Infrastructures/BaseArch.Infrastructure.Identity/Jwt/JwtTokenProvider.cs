using BaseArch.Application.Encryptions.Interfaces;
using BaseArch.Application.Identity.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.Timezones.Interfaces;
using BaseArch.Infrastructure.Identity.Jwt.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BaseArch.Infrastructure.Identity.Jwt
{
    /// <summary>
    /// Jwt token provider
    /// </summary>
    /// <param name="jwtOptions"></param>
    /// <param name="encryptor"></param>
    [DIService(DIServiceLifetime.Scoped)]
    public class JwtTokenProvider(IOptions<JwtOptions> jwtOptions, IEncryptionProvider encryptor, IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTimeProvider) : ITokenProvider
    {
        /// <summary>
        /// Current access token
        /// </summary>
        private string _currentAccessToken = string.Empty;

        /// <summary>
        /// "Bearer" scheme
        /// </summary>
        public string DefaultScheme { get; } = JwtBearerDefaults.AuthenticationScheme;

        /// <inheritdoc/>
        public string CreateAccessToken(IEnumerable<Claim> claims)
        {
            var token = CreateJwtToken(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        /// <inheritdoc/>
        public (string newAccessToken, string newRefreshToken) RenewAccessTokenFromRefreshToken(string refreshToken, string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(accessToken))
                throw new ArgumentException($"{nameof(accessToken)} is not valid", nameof(accessToken));

            var token = tokenHandler.ReadJwtToken(accessToken);
            var nameIdentifierClaim = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId);
            if (nameIdentifierClaim is null)
                throw new ArgumentException($"\"{nameof(JwtRegisteredClaimNames.NameId)}\" claim is not found from {nameof(accessToken)}", nameof(accessToken));

            try
            {
                var rawRefreshToken = encryptor.Decrypt(refreshToken, jwtOptions.Value.SecrectKey);
                var refreshTokenModel = JsonSerializer.Deserialize<JwtRefreshTokenModel>(rawRefreshToken);
                ArgumentNullException.ThrowIfNull(refreshTokenModel);

                if (refreshTokenModel.ExpiredAt < DateTimeOffset.UtcNow)
                {
                    throw new ArgumentException($"{nameof(refreshToken)} was expired", nameof(refreshToken));
                }

                if (refreshTokenModel.NameIdentifier != nameIdentifierClaim.Value)
                {
                    throw new ArgumentException($"\"{nameof(JwtRegisteredClaimNames.NameId)}\" claim is different between {nameof(accessToken)} and {nameof(refreshToken)}", nameof(refreshToken));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{nameof(refreshToken)} is not valid", nameof(refreshToken), ex);
            }

            var newAccessToken = CreateAccessToken(CloneClaim(token.Claims));
            var newRefreshToken = CreateRefreshToken(nameIdentifierClaim.Value);

            return (newAccessToken, newRefreshToken);
        }

        /// <inheritdoc/>
        public string CreateRefreshToken(string nameIdentifierClaimValue)
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            var randomKey = Convert.ToBase64String(randomNumber);

            var expiredAt = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.Value.RefreshTokenExpirationInMinute);

            var refreshTokenModel = new JwtRefreshTokenModel()
            {
                Token = randomKey,
                ExpiredAt = expiredAt,
                NameIdentifier = nameIdentifierClaimValue
            };

            return encryptor.Encrypt(JsonSerializer.Serialize(refreshTokenModel), jwtOptions.Value.SecrectKey);
        }
        public string GetAccessToken()
        {
            if (!string.IsNullOrEmpty(_currentAccessToken))
                return _currentAccessToken;

            if (httpContextAccessor.HttpContext is null)
                return string.Empty;

            var authorizationValue = httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault() ?? "";
            if (!authorizationValue.StartsWith(DefaultScheme))
                return string.Empty;

            _currentAccessToken = authorizationValue.Replace($"{DefaultScheme} ", "");
            return _currentAccessToken;
        }

        /// <inheritdoc/>
        public string GetUserKeyValue()
        {
            if (httpContextAccessor.HttpContext is null || httpContextAccessor.HttpContext.User is null)
                return string.Empty;

            if (httpContextAccessor.HttpContext.User.Identity is null)
                return string.Empty;

            var nameIdentifierClaim = httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == JwtRegisteredClaimNames.NameId);

            return nameIdentifierClaim?.Value ?? string.Empty;
        }

        /// <summary>
        /// Create Jwt token
        /// </summary>
        /// <param name="claims">List of <see cref="Claim"/></param>
        /// <returns><<see cref="JwtSecurityToken"/>/returns>
        private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims)
        {
            var expiration = dateTimeProvider.GetUtcNow().AddMinutes(jwtOptions.Value.AccessTokenExpirationInMinute);
            return new JwtSecurityToken(
               issuer: jwtOptions.Value.ValidIssuer,
               audience: jwtOptions.Value.ValidAudience,
               claims: claims,
               expires: expiration,
               signingCredentials: CreateSigningCredentials()
            );
        }

        /// <summary>
        /// Clone claim to avoid duplication
        /// </summary>
        /// <param name="claims">List of <see cref="Claim"/></param>
        /// <returns>List of <see cref="Claim"/></returns>
        private static List<Claim> CloneClaim(IEnumerable<Claim> claims)
        {
            var clonedClaims = new List<Claim>();
            foreach (var claim in claims)
            {
                switch (claim.Type)
                {
                    case JwtRegisteredClaimNames.Aud:
                    case JwtRegisteredClaimNames.Iss:
                    case JwtRegisteredClaimNames.Exp:
                        break;
                    case JwtRegisteredClaimNames.Jti:
                        clonedClaims.Add(new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        break;
                    case JwtRegisteredClaimNames.Iat:
                        clonedClaims.Add(new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));
                        break;
                    default:
                        clonedClaims.Add(claim);
                        break;
                }
            }

            return clonedClaims;
        }

        /// <summary>
        /// Create signing credentials
        /// </summary>
        /// <returns><see cref="SigningCredentials"/></returns>
        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.Value.SecrectKey)),
                SecurityAlgorithms.HmacSha256);
        }
    }
}