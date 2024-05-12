using Application.User.Dtos;

namespace Application.User.Services.Interfaces
{
    public interface IGetOrCreateUserService
    {
        Task<UserInfo> GetOrCreateUser(Guid id);
    }
}
