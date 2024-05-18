using Application.User.Dtos.Requests;
using Application.User.ExternalServices.Interfaces;
using Application.User.Services.Interfaces;
using Application.User.Validators.Interfaces;
using BaseArch.Domain.Attributes;
using BaseArch.Domain.Enums;
using BaseArch.Domain.Interfaces;
using Domain.Entities;
using Domain.Repositories.Interfaces;

namespace Application.User.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    internal class CreateUserService(IUnitOfWork unitOfWork, ICreateUserValidator validator, IGreetingClient greetingClient) : ICreateUserService
    {
        private readonly IUserRepository userRepository = unitOfWork.GetRepository<IUserRepository>();

        public async Task<Guid> CreateUser(CreateUserRequest request)
        {
            await validator.ValidateWithDatabaseAsync(request);

            UserEntity user = new(request.FirstName, request.LastName)
            {
                Id = Guid.NewGuid(),
                CreatedUserId = Guid.NewGuid(),
                UpdatedUserId = Guid.NewGuid()
            };

            await userRepository.Create(user).ConfigureAwait(false);
            await unitOfWork.SaveChangesAndCommit().ConfigureAwait(false);

            if (await greetingClient.CheckUserExisted($"{user.FirstName} {user.LastName}"))
            {
                return user.Id;
            }
            return Guid.Empty;
        }
    }
}
