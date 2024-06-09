using Application.Identity.Dtos;
using Application.Identity.Providers.Interfaces;
using BaseArch.Domain.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Providers
{
    [DIService(DIServiceLifetime.Scoped)]
    public class ClaimBuilderProvider : IClaimBuilderProvider
    {
        public IList<Claim> CreateClaims(IdentityUser identityUser)
        {
            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new(JwtRegisteredClaimNames.NameId, identityUser.Id!.ToString()!),
                new(JwtRegisteredClaimNames.Name, identityUser.UserName),
                new(JwtRegisteredClaimNames.Email, identityUser.Email),
            };

            if (identityUser.Roles is not null)
            {
                foreach (var role in identityUser.Roles)
                {
                    claims.Add(new(ClaimTypes.Role, role!.ToString()!));
                }
            }

            return claims;
        }
    }
}
