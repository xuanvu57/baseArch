using Application.User.Dtos.Requests;

namespace Application.User.Validators.Interfaces
{
    public interface ICreateUserValidator
    {
        Task ValidateWithDatabaseAsync(CreateUserRequest request);
    }
}
