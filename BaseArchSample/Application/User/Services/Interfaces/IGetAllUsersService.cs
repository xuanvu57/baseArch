using Application.User.Dtos;

namespace Application.User.Services.Interfaces
{
    public interface IGetAllUsersService
    {
        Task<IList<UserInfo>> GetAllUsers();
    }
}
