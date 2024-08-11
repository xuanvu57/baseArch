using Application.Identity.Services.Interfaces;
using Asp.Versioning;
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
    public class OAuthGoogleController(IOAuthGoogleService oAuthGoogleService) : BaseArchController
    {
        [HttpGet]
        public async Task<IResult> OAuthGoogle([FromQuery] string code, [FromQuery] string scope, [FromQuery] string authuser, [FromQuery] string prompt)
        {
            var callbackUrl = await oAuthGoogleService.GetTokenAndCreateCallbackUrl(code);

            return Results.Redirect(callbackUrl, true);
        }
    }
}
