using Application.Identity.Services.Interfaces;
using Asp.Versioning;
using BaseArch.Application.Models.Responses;
using BaseArch.Presentation.RestApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Identity.Controllers
{
    [AllowAnonymous]
    [Route(IdentityUriResource.Uri)]
    [ControllerName(IdentityUriResource.ControllerName)]
    [ApiVersion("1")]
    public class LoginController(ILoginService loginService) : BaseArchController
    {
        [HttpPost]
        public async Task<IResult> Login([FromBody] string UserName)
        {
            var response = await loginService.Login(UserName);

            if (response is null)
                return Results.Unauthorized();
            else
                return Results.Ok(Responses.From(response));
        }
    }
}
