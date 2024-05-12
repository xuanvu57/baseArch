using BaseArch.Domain.Enums;

namespace BaseArch.Domain.Attributes
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