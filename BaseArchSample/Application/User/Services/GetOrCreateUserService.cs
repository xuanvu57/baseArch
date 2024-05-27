using Application.User.Converters.Interfaces;
using Application.User.Dtos;
using Application.User.Services.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.Interfaces;
using Domain.Repositories.Interfaces;

namespace Application.User.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    internal class GetOrCreateUserService(IUnitOfWork unitOfWork,
        IUserEntityToUserInfoConverter userEntityToUserInfoConverter) : IGetOrCreateUserService
    {
        private readonly IUserRepository userRepository = unitOfWork.GetRepository<IUserRepository>();

        public async Task<UserInfo> GetOrCreateUser(Guid id)
        {
            var user = await userRepository.GetById(id);

            user ??= await userRepository.InitializeUser(id);
            await unitOfWork.SaveChangesAndCommit();

            return userEntityToUserInfoConverter.Convert(user);
        }
    }
}
