using Application.User.Converters.Interfaces;
using Application.User.Dtos;
using Application.User.Services.Interfaces;
using BaseArch.Domain.Attributes;
using BaseArch.Domain.Enums;
using BaseArch.Domain.Interfaces;
using Domain.Entities;

namespace Application.User.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    internal class GetAllUsersService(IUnitOfWork unitOfWork,
        IUserEntityToUserInfoConverter userEntityToUserInfoConverter) : IGetAllUsersService
    {
        private readonly IBaseRepository<UserEntity, Guid> genericUserRepository = unitOfWork.GetVirtualRepository<UserEntity, Guid, Guid>();

        public async Task<IList<UserInfo>> GetAllUsers()
        {
            var users = await genericUserRepository.Get();
            return users
                .Select(user => userEntityToUserInfoConverter.Convert(user))
                .ToList();
        }
    }
}
