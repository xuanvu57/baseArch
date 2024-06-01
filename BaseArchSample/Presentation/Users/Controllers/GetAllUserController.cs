using Application.User.Dtos;
using Application.User.Services.Interfaces;
using Asp.Versioning;
using BaseArch.Application.Models.Responses;
using BaseArch.Presentation.RestApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Users.Controllers
{
    [Route(UserUriResource.Uri)]
    [ControllerName(UserUriResource.ControllerName)]
    [ApiVersionNeutral]
    public class GetAllUserController(IGetAllUsersService userService) : BaseArchController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<UserInfo>>), 200)]
        public async Task<IResult> GetAllUsers()
        {
            var users = await userService.GetAllUsers();

            var response = Responses.From<IEnumerable<UserInfo>>(users, new PaginationResponseModel(1, 2, 3));
            return Results.Ok(response);
        }
    }
}
