using Application.Identity.Dtos;
using Application.Identity.Services.Interfaces;
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
    public class RefreshTokenController(IRefreshTokenService refreshTokenService) : BaseArchController
    {
        [HttpPost]
        public IResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var token = refreshTokenService.Refresh(request);

            if (token is null)
                return Results.Unauthorized();
            else
                return Results.Ok(Responses.From(token));
        }
    }
}
