namespace Presentation.Users
{
    public static class UserUriResource
    {
        public const string DomainName = "Users";

        public const string Uri = $"api/v{{version:apiVersion}}/{DomainName}/[action]";

        public const string ControllerName = $"{DomainName}";
    }
}
