using Application.Repositories.Interfaces;
using BaseArch.Application.Identity.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.Timezones.Interfaces;
using BaseArch.Infrastructure.EFCore.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [DIService(DIServiceLifetime.Scoped)]
    public sealed class UserRepository(SampleDBContext dbContext, ITokenProvider tokenProvider, IDateTimeProvider dateTimeProvider) : BaseRepository<UserEntity, Guid, Guid>(dbContext, tokenProvider, dateTimeProvider), IUserRepository
    {
        public async Task<UserEntity> GetFirstOrDefault()
        {
            var queryable = GetQueryable();
            var result = await queryable.FirstOrDefaultAsync();

            return result ??
                new UserEntity("Default", "Default")
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false
                };
        }

        public async Task<UserEntity> InitializeUser(Guid id)
        {
            var user = new UserEntity("Initialized First Name", "Initialized Last Name")
            {
                Id = id,
            };

            await Create(user);

            return user;
        }
    }
}
