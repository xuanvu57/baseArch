namespace BaseArch.Domain.DependencyInjection
{
    /// <summary>
    /// Enum of service lifetime for dependency injection
    /// </summary>
    public enum DIServiceLifetime
    {
        Singleton,
        Scoped,
        Transient
    }
}
