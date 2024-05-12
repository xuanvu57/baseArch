using BaseArch.Domain.Interfaces;
using Domain.Entities;

namespace Domain.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserEntity, Guid>
    {
        Task<UserEntity> GetFirstOrDefault();
        Task<UserEntity> InitializeUser(Guid id);
    }
}
