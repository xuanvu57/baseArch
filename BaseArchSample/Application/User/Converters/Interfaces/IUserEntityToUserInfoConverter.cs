using Application.User.Dtos;
using Domain.Entities;

namespace Application.User.Converters.Interfaces
{
    public interface IUserEntityToUserInfoConverter
    {
        UserInfo Convert(UserEntity user);
    }
}
