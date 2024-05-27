namespace BaseArch.Domain.DependencyInjection
{
    /// <summary>
    /// This service or component will be injected automatically
    /// </summary>
    /// <param name="lifetime"><see cref="DIServiceLifetime"/></param>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DIServiceAttribute(DIServiceLifetime lifetime) : Attribute
    {
        /// <summary>
        /// <see cref="DIServiceLifetime"/>
        /// </summary>
        public DIServiceLifetime Lifetime { get; } = lifetime;
    }
}