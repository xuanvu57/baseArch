using BaseArch.Application.Repositories.Interfaces;
using Domain.Entities;

namespace Application.User.Repositories.Interfaces
{
    public interface IUserMongoDbRepository : IBaseRepository<UserEntity, Guid>
    {
    }
}
