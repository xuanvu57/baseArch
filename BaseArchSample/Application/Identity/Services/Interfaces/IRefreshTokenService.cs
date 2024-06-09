using Application.Identity.Dtos.Requests;
using Application.Identity.Dtos.Responses;

namespace Application.Identity.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        TokenResponse Refresh(RefreshTokenRequest request);
    }
}
