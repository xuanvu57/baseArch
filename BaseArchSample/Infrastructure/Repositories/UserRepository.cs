using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.EFCore.Repositories;
using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [DIService(DIServiceLifetime.Scoped)]
    public sealed class UserRepository(SampleDBContext dbContext) : BaseRepository<UserEntity, Guid, Guid>(dbContext), IUserRepository
    {
        public async Task<UserEntity> GetFirstOrDefault()
        {
            var queryable = GetQueryable();
            var result = await queryable.FirstOrDefaultAsync();

            return result ??
                new UserEntity("Default", "Default")
                {
                    Id = Guid.NewGuid(),
                    CreatedDatetimeUtc = DateTime.UtcNow,
                    CreatedUserId = Guid.NewGuid(),
                    UpdatedDatetimeUtc = DateTime.UtcNow,
                    UpdatedUserId = Guid.NewGuid(),
                    IsDeleted = false
                };
        }

        public async Task<UserEntity> InitializeUser(Guid id)
        {
            var user = new UserEntity("Initialized First Name", "Initialized Last Name")
            {
                Id = id,
                CreatedUserId = Guid.NewGuid(),
                UpdatedUserId = Guid.NewGuid()
            };

            await Create(user);

            return user;
        }
    }
}
