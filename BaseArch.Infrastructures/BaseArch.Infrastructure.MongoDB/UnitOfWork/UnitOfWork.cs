using BaseArch.Application.Repositories.Interfaces;
using BaseArch.Domain.Entities;
using BaseArch.Infrastructure.MongoDB.DbContext.Interfaces;
using static BaseArch.Application.Repositories.Enums.DatabaseTypeEnums;

namespace BaseArch.Infrastructure.MongoDB.UnitOfWork
{
    /// <summary>
    /// MongoDb unit of work
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="dbContext"><see cref="IMongoDbContext"/></param>
    public abstract class UnitOfWork(IServiceProvider serviceProvider, IMongoDbContext dbContext) : IUnitOfWork
    {
        /// <summary>
        /// identify if object disposed
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// <see cref="RepositoryPool"/>
        /// </summary>
        private readonly RepositoryPool repositoryPool = new(serviceProvider, dbContext);

        /// <inheritdoc/>
        public DatabaseType DatabaseType { get; init; } = DatabaseType.MongoDb;

        /// <inheritdoc/>
        public TIRepository GetRepository<TIRepository>()
        {
            return repositoryPool.CreateRepository<TIRepository>();
        }

        /// <inheritdoc/>
        public IBaseRepository<TEntity, TKey> GetVirtualRepository<TEntity, TKey, TUserKey>() where TEntity : BaseEntity<TKey, TUserKey>
        {
            return repositoryPool.CreateRepository<TEntity, TKey, TUserKey>();
        }

        /// <inheritdoc/>
        public async Task<int> SaveChangesAndCommit()
        {
            try
            {
                await dbContext.CommitAsync();
                return 0;
            }
            catch
            {
                await dbContext.RollbackAsync();
                throw;
            }
        }

        #region Dispose
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~UnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disponse the object
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                dbContext.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}
