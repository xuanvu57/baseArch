using Application.User.Dtos.Requests;

namespace Application.User.Validators.Interfaces
{
    internal interface ICreateUserValidator
    {
        Task ValidateWithDatabaseAsync(CreateUserRequest request);
    }
}
