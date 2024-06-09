using Asp.Versioning;
using BaseArch.Application.Identity.Interfaces;
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
    public class LoginGoogleController(IEnumerable<ISsoProvider> ssoProviders) : BaseArchController
    {
        [HttpGet]
        public IResult LoginGoogle()
        {
            var loginUrl = ssoProviders.First(x => x.Name == "Google").GetLoginUrl();

            return Results.Redirect(loginUrl, true);
        }
    }
}
