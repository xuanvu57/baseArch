using Application.User.Dtos;
using Application.User.Services.Interfaces;
using Asp.Versioning;
using BaseArch.Presentation.RestApi;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Users.Controllers
{
    [Route(UserUriResource.Uri)]
    [ControllerName(UserUriResource.ControllerName)]
    [ApiVersionNeutral]
    public class GetAllUserController(IGetAllUsersService userService) : BaseArchController
    {
        [HttpGet]
        public async Task<IEnumerable<UserInfo>> GetAllUsers()
        {
            return await userService.GetAllUsers();
        }
    }
}
