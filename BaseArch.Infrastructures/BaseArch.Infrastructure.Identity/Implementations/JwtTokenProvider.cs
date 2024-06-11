using BaseArch.Application.Encryptions.Interfaces;
using BaseArch.Application.Identity.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.Identity.Models;
using BaseArch.Infrastructure.Identity.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BaseArch.Infrastructure.Identity.Implementations
{
    /// <summary>
    /// Jwt token provider
    /// </summary>
    /// <param name="jwtOptions"></param>
    /// <param name="encryptor"></param>
    [DIService(DIServiceLifetime.Scoped)]
    public class JwtTokenProvider(IOptions<JwtOptions> jwtOptions, IEncryptionProvider encryptor) : ITokenProvider
    {
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
                return ("", "");

            var token = tokenHandler.ReadJwtToken(accessToken);
            var nameIdentifier = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId);
            if (nameIdentifier is null)
                return ("", "");

            var rawRefreshToken = encryptor.Decrypt(refreshToken, jwtOptions.Value.SecrectKey);
            var refreshTokenModel = JsonSerializer.Deserialize<JwtRefreshTokenModel>(rawRefreshToken);

            if (refreshTokenModel is null ||
                refreshTokenModel.ExpiredAt < DateTimeOffset.UtcNow ||
                refreshTokenModel.NameIdentifier != nameIdentifier.Value)
                return ("", "");

            var newAccessToken = CreateAccessToken(CloneClaim(token.Claims));
            var newRefreshToken = CreateRefreshToken(refreshTokenModel.NameIdentifier);

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

        /// <summary>
        /// Create Jwt token
        /// </summary>
        /// <param name="claims">List of <see cref="Claim"/></param>
        /// <returns><<see cref="JwtSecurityToken"/>/returns>
        private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims)
        {
            var expiration = DateTime.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpirationInMinute);
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
        private IList<Claim> CloneClaim(IEnumerable<Claim> claims)
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