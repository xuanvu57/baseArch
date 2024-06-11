using Application.Identity.Dtos.Requests;
using Asp.Versioning;
using BaseArch.Application.Models.Responses;
using BaseArch.Presentation.RestApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Identity.Controllers
{
    [AllowAnonymous]
    [Route(IdentityUriResource.Uri)]
    [ControllerName(IdentityUriResource.ControllerName)]
    [ApiVersion("1")]
    public class LoginSsoCallbackController() : BaseArchController
    {
        [HttpGet]
        public IResult LoginSsoCallback([FromQuery] LoginSsoCallbackRequest parameters)
        {
            return Results.Ok(Responses.From(parameters));
        }
    }
}
