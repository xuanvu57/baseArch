namespace Application.User.ExternalServices.Interfaces
{
    public interface IGreetingClient
    {
        Task<bool> CheckUserExisted(string fullname);
    }
}
