namespace Application.User.ExternalServices.Interfaces
{
    public interface IGreetingClientOther
    {
        Task<string> TryToSayHello(string fullName);
    }
}
