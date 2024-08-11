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
    public class GetFirstUserController(IGetFirstUserService userService) : BaseArchController
    {
        [HttpGet]
        public async Task<UserInfo> GetFirstUser()
        {
            return await userService.GetFirstUser();
        }
    }
}
