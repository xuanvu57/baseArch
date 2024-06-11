namespace BaseArch.Application.Identity.Interfaces
{
    /// <summary>
    /// Generic identity user
    /// </summary>
    /// <typeparam name="TKey">Data type for user key</typeparam>
    /// <typeparam name="TRole">Data type for user role</typeparam>
    public interface IIdentityUser<TKey, TRole>
    {
        /// <summary>
        /// User id
        /// </summary>
        TKey Id { get; init; }

        /// <summary>
        /// User email
        /// </summary>
        string Email { get; init; }

        /// <summary>
        /// User name
        /// </summary>
        string UserName { get; init; }

        /// <summary>
        /// List of user roles
        /// </summary>
        IEnumerable<TRole> Roles { get; init; }
    }
}
