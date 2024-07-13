using MongoDB.Driver;

namespace BaseArch.Infrastructure.MongoDB.DbContext.Interfaces
{
    /// <summary>
    /// MongDb context interface
    /// </summary>
    public interface IMongoDbContext : IDisposable
    {
        /// <summary>
        /// Mongo database
        /// </summary>
        IMongoDatabase Database { get; }

        /// <summary>
        /// Session handler
        /// </summary>
        IClientSessionHandle? SessionHandle { get; }

        /// <summary>
        /// Begin a transaction
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commit a transaction
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rollback a transaction
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
