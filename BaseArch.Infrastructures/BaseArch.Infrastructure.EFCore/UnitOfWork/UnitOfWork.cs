using BaseArch.Application.Repositories.Interfaces;
using BaseArch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static BaseArch.Application.Repositories.Enums.DatabaseTypeEnums;

namespace BaseArch.Infrastructure.EFCore.UnitOfWork
{
    /// <summary>
    /// Entity framework unit of work
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="dbContext"><see cref="DbContext"/></param>
    /// <param name="databaseType"><see cref="DatabaseType"/></param>
    public abstract class UnitOfWork(IServiceProvider serviceProvider, DbContext dbContext, DatabaseType databaseType = DatabaseType.GeneralEfDb) : IUnitOfWork
    {
        /// <summary>
        /// identify if object disposed
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// <see cref="RepositoryPool"/>
        /// </summary>
        private readonly RepositoryPool _repositoryPool = new(serviceProvider, dbContext);

        /// <inheritdoc/>
        public DatabaseType DatabaseType { get; init; } = databaseType;

        /// <inheritdoc/>
        public TIRepository GetRepository<TIRepository>()
        {
            return _repositoryPool.CreateRepository<TIRepository>();
        }

        /// <inheritdoc/>
        public IBaseRepository<TEntity, TKey> GetVirtualRepository<TEntity, TKey, TUserKey>() where TEntity : BaseEntity<TKey, TUserKey>
        {
            return _repositoryPool.CreateRepository<TEntity, TKey, TUserKey>();
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
