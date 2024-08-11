namespace BaseArch.Domain.Attributes
{
    /// <summary>
    /// Allow class to be ignored by naming convention tests
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IgnoreNamingConventionAttribute : Attribute
    {
    }
}
