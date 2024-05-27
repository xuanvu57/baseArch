using BaseArch.Domain.Entities;

namespace BaseArch.Domain.Repositories.Interfaces
{
    /// <summary>
    /// Unit of work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Get an undefined repository as base repository
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <typeparam name="TKey">Type of entity's key</typeparam>
        /// <typeparam name="TUserKey">Type of user's key</typeparam>
        /// <returns>Repository for entity</returns>
        IBaseRepository<TEntity, TKey> GetVirtualRepository<TEntity, TKey, TUserKey>() where TEntity : BaseEntity<TKey, TUserKey>;

        /// <summary>
        /// Get the defined repository
        /// </summary>
        /// <typeparam name="TRepository">Type of repository</typeparam>
        /// <returns>Repository</returns>
        TRepository GetRepository<TRepository>();

        /// <summary>
        /// Save changes into database and commit the transaction. It throws exception when transaction fails and rollbacks
        /// </summary>
        /// <returns>Number of effected records</returns>
        Task<int> SaveChangesAndCommit();
    }
}
