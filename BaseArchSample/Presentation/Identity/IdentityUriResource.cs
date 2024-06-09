namespace Presentation.Identity
{
    public static class IdentityUriResource
    {
        public const string DomainName = "Identity";

        public const string Uri = $"api/v{{version:apiVersion}}/{DomainName}/[action]";

        public const string ControllerName = $"{DomainName}";
    }
}
