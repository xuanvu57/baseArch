using Application.User.Repositories.Interfaces;
using BaseArch.Application.Identity.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.Timezones.Interfaces;
using BaseArch.Infrastructure.MongoDB.DbContext.Interfaces;
using BaseArch.Infrastructure.MongoDB.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    [DIService(DIServiceLifetime.Scoped)]
    public class UserMongoDbRepository(IMongoDbContext dbContext, ITokenProvider tokenProvider, IDateTimeProvider dateTimeProvider)
        : BaseRepository<UserEntity, Guid, Guid>(dbContext, tokenProvider, dateTimeProvider), IUserMongoDbRepository
    {
    }
}
