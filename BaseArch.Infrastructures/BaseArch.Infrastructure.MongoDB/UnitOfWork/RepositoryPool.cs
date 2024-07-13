using BaseArch.Application.Repositories.Interfaces;
using BaseArch.Domain.Entities;
using BaseArch.Infrastructure.MongoDB.DbContext.Interfaces;
using BaseArch.Infrastructure.MongoDB.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.MongoDB.UnitOfWork
{
    /// <summary>
    /// Pool to create and store repository for Unit of work
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    /// <param name="dbContext"><see cref="IMongoDbContext"/></param>
    public sealed class RepositoryPool(IServiceProvider serviceProvider, IMongoDbContext dbContext)
    {
        /// <summary>
        /// Pool of repositories
        /// </summary>
        private readonly Dictionary<string, object> _repositoryPool = [];

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

            if (!_repositoryPool.TryGetValue(keyOfIRepository, out var repository))
            {
                var typeOfRepository = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assemble => assemble.GetTypes())
                    .First(type => typeOfIRepository.IsAssignableFrom(type) && type.IsClass);

                repository = ActivatorUtilities.CreateInstance(serviceProvider, typeOfRepository, dbContext);
                _repositoryPool.Add(keyOfIRepository, repository);
            }

            return (TIRepository)repository;
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

            if (!_repositoryPool.TryGetValue(keyOfRepository, out var repository))
            {
                repository = ActivatorUtilities.CreateInstance(serviceProvider, typeof(BaseRepository<TEntity, TKey, TUserKey>), dbContext);
                _repositoryPool.Add(keyOfRepository, repository);
            }

            return (IBaseRepository<TEntity, TKey>)repository;
        }
    }
}
