using Application.User.Dtos.Requests;

namespace Application.User.Services.Interfaces
{
    public interface ICreateUserService
    {
        Task<Guid> CreateUser(CreateUserRequest request);
    }
}
