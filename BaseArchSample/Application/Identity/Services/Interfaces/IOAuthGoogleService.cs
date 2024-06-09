namespace Application.Identity.Services.Interfaces
{
    public interface IOAuthGoogleService
    {
        Task<string> GetTokenAndCreateCallbackUrl(string authorizationCode);
    }
}
