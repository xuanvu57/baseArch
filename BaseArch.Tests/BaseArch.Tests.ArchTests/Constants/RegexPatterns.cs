namespace BaseArch.Tests.ArchTests.Constants
{
    public static class RegexPatterns
    {
        public const string PascalCase = @"^[A-Z](([A-Z]{1,2}[a-z0-9]+)+([A-Z]{1,3}[a-z0-9]+)*[A-Z]{0,3}|([a-z0-9]+[A-Z]{0,3})*|[A-Z]{1,2})$";
        public const string PascalCaseForGenericClass = @"^[A-Z](([A-Z]{1,2}[a-z0-9]+)+([A-Z]{1,3}[a-z0-9]+)*[A-Z]{0,3}|([a-z0-9]+[A-Z]{0,3})*|[A-Z]{1,2})`[0-9]$";
        public const string PascalCaseForInterface = @"^I[A-Z](([A-Z]{1,2}[a-z0-9]+)+([A-Z]{1,3}[a-z0-9]+)*[A-Z]{0,3}|([a-z0-9]+[A-Z]{0,3})*|[A-Z]{1,2})$";
        public const string PascalCaseForGenericInterface = @"^I[A-Z](([A-Z]{1,2}[a-z0-9]+)+([A-Z]{1,3}[a-z0-9]+)*[A-Z]{0,3}|([a-z0-9]+[A-Z]{0,3})*|[A-Z]{1,2})`[0-9]$";
        public const string CamelCase = @"^[a-z][a-z0-9]*(([A-Z]{1,3}[a-z0-9]+)*[A-Z]{0,3}|([a-z0-9]+[A-Z]{1,3})*|[A-Z]{1,3})$";
        public const string CamelCaseAndStartWithUnderscore = @"^_[a-z][a-z0-9]*(([A-Z]{1,3}[a-z0-9]+)*[A-Z]{0,3}|([a-z0-9]+[A-Z]{1,3})*|[A-Z]{1,3})$";
    }
}
