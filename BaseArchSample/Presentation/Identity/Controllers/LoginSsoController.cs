using Asp.Versioning;
using BaseArch.Application.Identity.Interfaces;
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
    public class LoginSsoController(IEnumerable<ISsoProvider> ssoProviders) : BaseArchController
    {
        [HttpGet]
        public IResult LoginSso([FromQuery] string ssoProvider)
        {
            var loginUrl = ssoProviders.First(x => x.Name.Equals(ssoProvider, StringComparison.CurrentCultureIgnoreCase)).GetLoginUrl();

            return Results.Redirect(loginUrl, true);
        }
    }
}
