using BaseArch.Application.Repositories.Interfaces;
using Domain.Entities;

namespace Application.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserEntity, Guid>
    {
        Task<UserEntity> GetFirstOrDefault();
        Task<UserEntity> InitializeUser(Guid id);
    }
}
