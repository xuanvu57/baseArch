using Application.Repositories.Interfaces;
using Application.User.Dtos.Messages;
using Application.User.Dtos.Requests;
using Application.User.ExternalServices.Interfaces;
using Application.User.Repositories.Interfaces;
using Application.User.Services.Interfaces;
using Application.User.Validators.Interfaces;
using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Application.Repositories.Interfaces;
using BaseArch.Domain.DependencyInjection;
using CaseExtensions;
using Domain.Entities;
using static BaseArch.Application.Repositories.Enums.DatabaseTypeEnums;

namespace Application.User.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    public class CreateUserService : ICreateUserService
    {
        private readonly IUnitOfWork _efUnitOfWork;
        private readonly IUnitOfWork _mongoDbUnitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserMongoDbRepository _userMongoDbRepository;
        private readonly ICreateUserValidator _validator;
        private readonly IGreetingClient _greetingClient;
        private readonly IGreetingClientOther _greetingClientOther;
        private readonly IPublisher _publisher;
        private readonly ISender _sender;

        public CreateUserService(IEnumerable<IUnitOfWork> unitOfWorks,
            ICreateUserValidator validator,
            IGreetingClient greetingClient,
            IGreetingClientOther greetingClientOther,
            IPublisher publisher,
            ISender sender)
        {
            _efUnitOfWork = unitOfWorks.First(x => x.DatabaseType == DatabaseType.GeneralEfDb);
            _mongoDbUnitOfWork = unitOfWorks.First(x => x.DatabaseType == DatabaseType.MongoDb);

            _userRepository = _efUnitOfWork.GetRepository<IUserRepository>();

            _userMongoDbRepository = _mongoDbUnitOfWork.GetRepository<IUserMongoDbRepository>();

            _validator = validator;
            _greetingClient = greetingClient;
            _greetingClientOther = greetingClientOther;
            _publisher = publisher;
            _sender = sender;
        }

        public async Task<Guid> CreateUser(CreateUserRequest request)
        {
            await _validator.ValidateWithDatabaseAsync(request);

            UserEntity user = new(request.FirstName, request.LastName)
            {
                Id = Guid.NewGuid(),
            };

            await _userRepository.Create(user).ConfigureAwait(false);
            await _efUnitOfWork.SaveChangesAndCommit().ConfigureAwait(false);

            var count = await _userMongoDbRepository.Count().ConfigureAwait(false);
            if (count == 0)
            {
                await _userMongoDbRepository.Create(user).ConfigureAwait(false);
                await _mongoDbUnitOfWork.SaveChangesAndCommit().ConfigureAwait(false);
            }

            var responseFromGreetingClient = await _greetingClient.TryToSayHello($"{user.FirstName} {user.LastName}");
            var responseFromGreetingClientOther = await _greetingClientOther.TryToSayHello($"{user.FirstName} {user.LastName}");
            if (string.IsNullOrEmpty(responseFromGreetingClient) ||
                string.IsNullOrEmpty(responseFromGreetingClientOther))
            {
                return Guid.Empty;
            }

            await _publisher.Publish(new UserCreatedPublishedMessage(user.Id));

            await _publisher.Publish(new UserCreatedCustomizeMessage(Guid.NewGuid(), user.Id));

            await _sender.Send(new UserCreatedSentMessage(user.Id, $"{user.FirstName} {user.LastName}"), $"queue:{typeof(UserCreatedSentMessage).Name.ToKebabCase()}");

            return user.Id;
        }
    }
}
