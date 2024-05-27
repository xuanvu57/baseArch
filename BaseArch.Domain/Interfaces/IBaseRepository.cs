using BaseArch.Domain.RestApi;
using System.Linq.Expressions;

namespace BaseArch.Domain.Interfaces
{
    /// <summary>
    /// The generic repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IBaseRepository<TEntity, TKey>
    {
        /// <summary>
        /// Get IQueryable for the data set
        /// </summary>
        /// <param name="predicate">Default predication</param>
        /// <param name="includeDeletedRecords">Default exclusion for soft-deleted records; otherwise set to be True</param>
        /// <returns><see cref="IQueryable{TEntity}"/></returns>
        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false);

        /// <summary>
        /// Create new record
        /// </summary>
        /// <param name="entity">Entity to create</param>
        Task Create(TEntity entity);

        /// <summary>
        /// Create list of new records
        /// </summary>
        /// <param name="entities">List of entities to create</param>
        Task CreateMany(IEnumerable<TEntity> entities);

        /// <summary>
        /// Get list of records from expression
        /// </summary>
        /// <param name="predicate">Prediction to search</param>
        /// <param name="includeDeletedRecords">Default exclusion for soft-deleted records; otherwise set to be True</param>
        /// <returns>List of entities</returns>
        Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false);

        /// <summary>
        /// Get list of records from Query model
        /// </summary>
        /// <param name="queryModel"><see cref="QueryModel"/></param>
        /// <param name="includeDeletedRecords">Default exclusion for soft-deleted records; otherwise set to be True</param>
        /// <returns>List of entities</returns>
        Task<IList<TEntity>> Get(QueryModel queryModel, bool includeDeletedRecords = false);

        /// <summary>
        /// Get record by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Entity</returns>
        Task<TEntity?> GetById(TKey id);

        /// <summary>
        /// Count number of record from expression
        /// </summary>
        /// <param name="predicate">Prediction to search</param>
        /// <param name="includeDeletedRecords">Default exclusion for soft-deleted records; otherwise set to be True</param>
        /// <returns>Number of records</returns>
        Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false);

        /// <summary>
        /// Count number of record Query model
        /// </summary>
        /// <param name="queryModel"><see cref="QueryModel"/></param>
        /// <param name="includeDeletedRecords">Default exclusion for soft-deleted records; otherwise set to be True</param>
        /// <returns>Number of records</returns>
        Task<int> Count(QueryModel queryModel, bool includeDeletedRecords = false);

        /// <summary>
        /// Update existing record
        /// </summary>
        /// <param name="entity">Entity to update</param>
        Task Update(TEntity entity);

        /// <summary>
        /// Update existing records
        /// </summary>
        /// <param name="entities">List of entities to update</param>
        Task UpdateMany(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete existing record. This is a soft-delete method
        /// </summary>
        /// <param name="key">Key to delete</param>
        Task Delete(TKey key);

        /// <summary>
        /// Delete list of existing records. This is a soft-delete method
        /// </summary>
        /// <param name="keys">List of keys to delete</param>
        Task DeleteMany(IEnumerable<TKey> keys);
    }
}