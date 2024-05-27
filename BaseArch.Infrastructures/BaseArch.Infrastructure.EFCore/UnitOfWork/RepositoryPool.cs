using BaseArch.Domain.Entities;
using BaseArch.Domain.Repositories.Interfaces;
using BaseArch.Infrastructure.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.EFCore.UnitOfWork
{
    /// <summary>
    /// Pool to create and store repository for Unit of work
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="dbContext"><see cref="DbContext"/></param>
    public sealed class RepositoryPool(IServiceProvider serviceProvider, DbContext dbContext)
    {
        /// <summary>
        /// Pool of repositories
        /// </summary>
        private readonly Dictionary<string, object> repositoryPool = [];

        /// <summary>
        /// Create the defined repository
        /// </summary>
        /// <typeparam name="TIRepository">Type of repository interface</typeparam>
        /// <returns>Repository interface</returns>
        public TIRepository CreateRepository<TIRepository>()
        {
            var typeOfIRepository = typeof(TIRepository);
            var keyOfIRepository = typeof(TIRepository).Name;

            if (typeOfIRepository.GenericTypeArguments.Length > 0)
            {
                var genericTypes = typeOfIRepository.GenericTypeArguments.Select(t => t.Name);
                keyOfIRepository += $"{string.Join("|", genericTypes)}";
            }

            if (!repositoryPool.ContainsKey(keyOfIRepository))
            {
                var typeOfRepository = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assemble => assemble.GetTypes())
                    .First(type => typeOfIRepository.IsAssignableFrom(type) && type.IsClass);

                var repository = ActivatorUtilities.CreateInstance(serviceProvider, typeOfRepository, dbContext);
                if (repository is not null)
                {
                    repositoryPool.Add(keyOfIRepository, repository);
                }
            }

            return (TIRepository)repositoryPool[keyOfIRepository];
        }

        /// <summary>
        /// Create the virtual repository
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <typeparam name="TKey">Type of entity's key</typeparam>
        /// <typeparam name="TUserKey">Type of user's key</typeparam>
        /// <returns><see cref="IBaseRepository{TEntity, TKey}"/></returns>
        public IBaseRepository<TEntity, TKey> CreateRepository<TEntity, TKey, TUserKey>() where TEntity : BaseEntity<TKey, TUserKey>
        {
            var typeOfEntity = typeof(TEntity);
            var keyOfRepository = typeOfEntity.Name;

            if (!repositoryPool.ContainsKey(keyOfRepository))
            {
                var repository = new BaseRepository<TEntity, TKey, TUserKey>(dbContext);
                repositoryPool.Add(keyOfRepository, repository);
            }

            return (IBaseRepository<TEntity, TKey>)repositoryPool[keyOfRepository];
        }
    }
}
