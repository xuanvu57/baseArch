using BaseArch.Domain.Entities;
using BaseArch.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BaseArch.Infrastructure.EFCore.UnitOfWork
{
    /// <inheritdoc/>
    public abstract class UnitOfWork(IServiceProvider serviceProvider, DbContext dbContext) : IUnitOfWork
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
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var numberOfEffectedRows = await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return numberOfEffectedRows;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

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
    }
}
