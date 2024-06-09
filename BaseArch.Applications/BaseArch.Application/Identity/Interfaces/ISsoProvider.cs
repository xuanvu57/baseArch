namespace BaseArch.Application.Identity.Interfaces
{
    public interface ISsoProvider
    {
        string Name { get; }
        string GetLoginUrl();
        Task<string> GetToken(string authorizationCode);
        Task<string> RenewToken(string refreshToken);
        Task<string> GetUserInfo(string accessToken);
    }
}
