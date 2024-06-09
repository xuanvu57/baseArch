using Application.Identity.Dtos;
using Application.Identity.Dtos.Responses;
using Application.Identity.Providers.Interfaces;
using Application.Identity.Services.Interfaces;
using BaseArch.Application.Identity.Interfaces;
using BaseArch.Application.Repositories.Interfaces;
using BaseArch.Domain.DependencyInjection;
using Domain.Entities;

namespace Application.Identity.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    public class LoginService(IUnitOfWork unitOfWork, ITokenProvider tokenProvider, IClaimBuilderProvider claimBuilderProvider) : ILoginService
    {
        public async Task<TokenResponse> Login(string UserName)
        {
            var userRepository = unitOfWork.GetVirtualRepository<UserEntity, Guid, Guid>();
            var users = await userRepository.Get();

            var user = users.FirstOrDefault(x => x.FirstName.Contains(UserName) || x.LastName.Contains(UserName));

            if (user is not null)
            {
                var identityUser = new IdentityUser()
                {
                    Id = user.Id,
                    Email = $"{user.FirstName}@gmail.com",
                    UserName = $"{user.FirstName}{user.LastName}"
                };

                var claims = claimBuilderProvider.CreateClaims(identityUser);
                var accessToken = tokenProvider.CreateAccessToken(claims);
                var refreshToken = tokenProvider.CreateRefreshToken(identityUser.Id.ToString());

                return new TokenResponse()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }

            return null;
        }
    }
}
