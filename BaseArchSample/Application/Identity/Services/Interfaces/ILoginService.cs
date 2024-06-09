using Application.Identity.Dtos.Responses;

namespace Application.Identity.Services.Interfaces
{
    public interface ILoginService
    {
        Task<TokenResponse> Login(string UserName);
    }
}
