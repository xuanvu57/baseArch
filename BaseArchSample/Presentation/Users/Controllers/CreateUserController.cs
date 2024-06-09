using Application.User.Dtos.Requests;
using Application.User.Services.Interfaces;
using Asp.Versioning;
using BaseArch.Application.Models.Responses;
using BaseArch.Presentation.RestApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Users.Controllers
{
    //[Route("api/v{version:apiVersion}/[controller]")]
    //[SwaggerTag("This is summary for controller")]
    [Route(UserUriResource.Uri)]
    [ControllerName(UserUriResource.ControllerName)]
    [ApiVersion("1")]
    public class CreateUserController(ICreateUserService userService) : BaseArchController
    {
        //[SwaggerOperation(Summary = "This is summary for action", Description = "This is summary in detail for action")]
        //[MapToApiVersion("1")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResult> Create([FromBody] CreateUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var id = await userService.CreateUser(request);

            var response = Responses.From<Guid>(id);
            return Results.Ok(response);
        }
    }
}
