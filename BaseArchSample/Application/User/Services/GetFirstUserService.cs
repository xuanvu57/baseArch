using Application.Repositories.Interfaces;
using Application.User.Converters.Interfaces;
using Application.User.Dtos;
using Application.User.Services.Interfaces;
using BaseArch.Application.Repositories.Interfaces;
using BaseArch.Domain.DependencyInjection;

namespace Application.User.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    public class GetFirstUserService(IUnitOfWork unitOfWork,
        IUserEntityToUserInfoConverter userEntityToUserInfoConverter) : IGetFirstUserService
    {
        private readonly IUserRepository userRepository = unitOfWork.GetRepository<IUserRepository>();

        public async Task<UserInfo> GetFirstUser()
        {
            var user = await userRepository.GetFirstOrDefault();
            return userEntityToUserInfoConverter.Convert(user);
        }
    }
}
