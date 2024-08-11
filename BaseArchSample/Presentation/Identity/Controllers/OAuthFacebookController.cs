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
    public class OAuthFacebookController(IOAuthFacebookService oAuthFacebookService) : BaseArchController
    {
        [HttpGet]
        public async Task<IResult> OAuthFacebook([FromQuery] string code, [FromQuery] string state, [FromQuery] string error_reason, [FromQuery] string error, [FromQuery] string error_description)
        {
            var callbackUrl = await oAuthFacebookService.GetTokenAndCreateCallbackUrl(code, state, string.IsNullOrEmpty(error));

            return Results.Redirect(callbackUrl, true);
        }
    }
}
