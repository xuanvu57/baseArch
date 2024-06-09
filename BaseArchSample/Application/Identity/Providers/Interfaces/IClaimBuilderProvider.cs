using Application.Identity.Dtos;
using System.Security.Claims;

namespace Application.Identity.Providers.Interfaces
{
    public interface IClaimBuilderProvider
    {
        IList<Claim> CreateClaims(IdentityUser identityUser);
    }
}
