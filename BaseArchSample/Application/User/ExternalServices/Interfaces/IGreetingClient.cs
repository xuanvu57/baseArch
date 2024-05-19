namespace Application.User.ExternalServices.Interfaces
{
    public interface IGreetingClient
    {
        Task<string> TryToSayHello(string fullName);
    }
}
