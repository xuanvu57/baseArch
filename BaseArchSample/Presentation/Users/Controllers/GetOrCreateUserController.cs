using Application.User.Dtos;
using Application.User.Services.Interfaces;
using Asp.Versioning;
using BaseArch.Presentation.RestApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Users.Controllers
{
    [Route(UserUriResource.Uri)]
    [ControllerName(UserUriResource.DomainName)]
    [ApiVersion("2")]
    public class GetOrCreateUserController(IGetOrCreateUserService userService) : BaseArchController
    {
        [HttpPost]
        public async Task<UserInfo> GetOrCreateUser(Guid id)
        {
            return await userService.GetOrCreateUser(id);
        }
    }
}
