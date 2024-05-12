using Application.User.Dtos;

namespace Application.User.Services.Interfaces
{
    public interface IGetFirstUserService
    {
        Task<UserInfo> GetFirstUser();
    }
}
