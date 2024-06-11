namespace Application.Identity.Services.Interfaces
{
    public interface IOAuthFacebookService
    {
        Task<string> GetTokenAndCreateCallbackUrl(string code, string state, bool success);
    }
}
