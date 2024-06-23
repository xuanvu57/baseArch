using BaseArch.Application.Models.Requests;
using System.Linq.Expressions;

namespace BaseArch.Application.Repositories.Interfaces
{
    /// <summary>
    /// The generic repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IBaseRepository<TEntity, TKey>
    {
        /// <summary>
        /// Create new record
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        Task Create(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create list of new records
        /// </summary>
        /// <param name="entities">List of entities to create</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        Task CreateMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get list of records from expression
        /// </summary>
        /// <param name="predicate">Prediction to search</param>
        /// <param name="includeDeletedRecords">Default exclusion for soft-deleted records; otherwise set to be True</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>List of entities</returns>
        Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get list of records from Query model
        /// </summary>
        /// <param name="queryModel"><see cref="QueryModel"/></param>
        /// <param name="includeDeletedRecords">Default exclusion for soft-deleted records; otherwise set to be True</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>List of entities</returns>
        Task<IList<TEntity>> Get(QueryModel queryModel, bool includeDeletedRecords = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get record by id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Entity</returns>
        Task<TEntity?> GetById(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Count number of record from expression
        /// </summary>
        /// <param name="predicate">Prediction to search</param>
        /// <param name="includeDeletedRecords">Default exclusion for soft-deleted records; otherwise set to be True</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Number of records</returns>
        Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Count number of record Query model
        /// </summary>
        /// <param name="queryModel"><see cref="QueryModel"/></param>
        /// <param name="includeDeletedRecords">Default exclusion for soft-deleted records; otherwise set to be True</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Number of records</returns>
        Task<int> Count(QueryModel queryModel, bool includeDeletedRecords = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update existing record
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        Task Update(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update existing records
        /// </summary>
        /// <param name="entities">List of entities to update</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        Task UpdateMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete existing record. This is a soft-delete method
        /// </summary>
        /// <param name="entities">List of entities to delete</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        Task Delete(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete list of existing records. This is a soft-delete method
        /// </summary>
        /// <param name="entities">List of entities to delete</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        Task DeleteMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}