using Application.User.Converters.Interfaces;
using Application.User.Dtos;
using AutoMapper;
using BaseArch.Application.AutoMapper.Extensions;
using BaseArch.Domain.DependencyInjection;
using Domain.Entities;

namespace Application.User.Converters
{
    [DIService(DIServiceLifetime.Scoped)]
    public class UserEntityToUserInfoConverter : IUserEntityToUserInfoConverter
    {
        private readonly IMapper mapper;
        public UserEntityToUserInfoConverter()
        {
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<UserEntity, UserInfo>()
                        .ForMember(des => des.FullName, src => $"{src.FirstName} {src.LastName}")
                        .ForMember(des => des.LatestUpdatedDate, src => src.UpdatedDatetimeUtc.ToLongTimeString())
            );

            mapper = config.CreateMapper();
        }

        public UserInfo Convert(UserEntity user)
        {
            return mapper.Map<UserInfo>(user);
        }
    }
}
