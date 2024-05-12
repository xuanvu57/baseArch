using Application.User.Dtos.Requests;
using Application.User.Services.Interfaces;
using Asp.Versioning;
using BaseArch.Presentation.RestApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Presentation.Users.Controllers
{
    //[Route("api/v{version:apiVersion}/[controller]")]
    //[SwaggerTag("This is summary for controller")]
    [Route(UserUriResource.Uri)]
    [ControllerName(UserUriResource.ControllerName)]
    [ApiVersion("1")]
    public class CreateUserController(ICreateUserService userService, ILogger<CreateUserController> logger, IConfiguration configuration) : BaseArchController
    {
        //[SwaggerOperation(Summary = "This is summary for action", Description = "This is summary in detail for action")]
        //[MapToApiVersion("1")]
        [HttpPost]
        public async Task<Guid> Create([FromBody] CreateUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var test = configuration.GetValue<string>("Configuration:FromEnvironment");
            logger.LogInformation("It start to create user {firstname} {lastname} with {Test}", request.FirstName, request.LastName, test);

            return await userService.CreateUser(request);
        }
    }
}
