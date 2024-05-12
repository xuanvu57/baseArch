using Application.User.Converters.Interfaces;
using Application.User.Dtos;
using Application.User.Services.Interfaces;
using BaseArch.Domain.Attributes;
using BaseArch.Domain.Enums;
using BaseArch.Domain.Interfaces;
using Domain.Repositories.Interfaces;

namespace Application.User.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    internal class GetFirstUserService(IUnitOfWork unitOfWork,
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
