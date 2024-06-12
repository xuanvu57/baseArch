using Application.Repositories.Interfaces;
using Application.User.Dtos.Messages;
using Application.User.Dtos.Requests;
using Application.User.ExternalServices.Interfaces;
using Application.User.Services.Interfaces;
using Application.User.Validators.Interfaces;
using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Application.Repositories.Interfaces;
using BaseArch.Domain.DependencyInjection;
using CaseExtensions;
using Domain.Entities;

namespace Application.User.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    public class CreateUserService(IUnitOfWork unitOfWork,
        ICreateUserValidator validator,
        IGreetingClient greetingClient,
        IGreetingClientOther greetingClientOther,
        IPublisher publisher,
        ISender sender) : ICreateUserService
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

            var responseFromGreetingClient = await greetingClient.TryToSayHello($"{user.FirstName} {user.LastName}");
            var responseFromGreetingClientOther = await greetingClientOther.TryToSayHello($"{user.FirstName} {user.LastName}");
            if (string.IsNullOrEmpty(responseFromGreetingClient) ||
                string.IsNullOrEmpty(responseFromGreetingClientOther))
            {
                return Guid.Empty;
            }

            await publisher.Publish(new UserCreatedPublishedMessage(user.Id));

            await publisher.Publish(new UserCreatedCustomizeMessage(Guid.NewGuid(), user.Id));

            await sender.Send(new UserCreatedSentMessage(user.Id, $"{user.FirstName} {user.LastName}"), $"queue:{typeof(UserCreatedSentMessage).Name.ToKebabCase()}");

            return user.Id;
        }
    }
}
