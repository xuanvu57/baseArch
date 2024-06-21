using BaseArch.Application.Identity.Interfaces;
using BaseArch.Application.Models;
using BaseArch.Application.Models.Requests;
using BaseArch.Application.Repositories.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.Entities;
using BaseArch.Infrastructure.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseArch.Infrastructure.EFCore.Repositories
{
    /// <inheritdoc/>
    [DIService(DIServiceLifetime.Scoped)]
    public class BaseRepository<TEntity, TKey, TUserKey>(DbContext dbContext, ITokenProvider tokenProvider) : IBaseRepository<TEntity, TKey> where TEntity : BaseEntity<TKey, TUserKey>
    {
        /// <summary>
        /// The DbSet of TEntity
        /// </summary>
        private readonly DbSet<TEntity> dbSet = dbContext.Set<TEntity>();

        /// <inheritdoc/>
        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false)
        {
            var queryable = dbSet.AsQueryable();

            if (predicate is not null)
                queryable = queryable.Where(predicate);

            if (!includeDeletedRecords)
                queryable = queryable.Where(e => !e.IsDeleted);

            return queryable;
        }

        /// <inheritdoc/>
        public async Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false, CancellationToken cancellationToken = default)
        {
            return await GetQueryable(predicate, includeDeletedRecords).CountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<int> Count(QueryModel queryModel, bool includeDeletedRecords = false, CancellationToken cancellationToken = default)
        {
            var queryable = GetQueryable(null, includeDeletedRecords);

            if (queryModel is not null)
            {
                if (queryModel.Filters?.FirstOrDefault() is not null)
                {
                    queryable = queryable.GenerateANDFilterExpression(queryModel.Filters);
                }

                if (!string.IsNullOrEmpty(queryModel.Search?.SearchText))
                {
                    queryable = queryable.GenerateORFilterExpression(queryModel.Search.FieldNames, queryModel.Search.SearchText);
                }
            }

            return await queryable.CountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false, CancellationToken cancellationToken = default)
        {
            return await GetQueryable(predicate, includeDeletedRecords).ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IList<TEntity>> Get(QueryModel queryModel, bool includeDeletedRecords = false, CancellationToken cancellationToken = default)
        {
            var queryable = GetQueryable(null, includeDeletedRecords);

            if (queryModel is not null)
            {
                if (queryModel.Filters?.FirstOrDefault() is not null)
                {
                    queryable = queryable.GenerateANDFilterExpression(queryModel.Filters);
                }

                if (!string.IsNullOrEmpty(queryModel.Search?.SearchText))
                {
                    queryable = queryable.GenerateORFilterExpression(queryModel.Search.FieldNames, queryModel.Search.SearchText);
                }

                if (queryModel.Sort is not null && !string.IsNullOrWhiteSpace(queryModel.Sort.SortBy))
                {
                    queryable = queryModel.Sort.SortOrder switch
                    {
                        SortOrderConst.Desc => queryable.CustomizedOrderByDescending(queryModel.Sort.SortBy),
                        _ => queryable.CustomizedOrderBy(queryModel.Sort.SortBy)
                    };
                }

                if (queryModel.Pagination is not null)
                {
                    queryable = queryable
                        .Skip((queryModel.Pagination.PageNumber <= 0 ? 1 : queryModel.Pagination.PageNumber - 1) * queryModel.Pagination.PageSize)
                        .Take(queryModel.Pagination.PageSize);
                }
            }

            return await queryable.ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetById(TKey id, CancellationToken cancellationToken = default)
        {
            if (id is null)
                return null;

            return await GetQueryable(e => e.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var createdEntity = entity.SetCreation<TEntity>(tokenProvider.GetUserKeyValue(), DateTime.UtcNow);

            await dbSet.AddAsync(createdEntity, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CreateMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (var entity in entities)
            {
                await Create(entity, cancellationToken);
            }
        }

        /// <inheritdoc/>
        public async Task Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var deletedEntity = entity.SetDeletion<TEntity>(tokenProvider.GetUserKeyValue(), DateTime.UtcNow);

            dbSet.Update(deletedEntity);

            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task DeleteMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (var entity in entities)
            {
                await Delete(entity, cancellationToken);
            }

            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var updatedEntity = entity.SetModification<TEntity>(tokenProvider.GetUserKeyValue(), DateTime.UtcNow);

            dbSet.Update(updatedEntity);

            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task UpdateMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (var entity in entities)
            {
                await Update(entity, cancellationToken);
            }

            await Task.CompletedTask;
        }
    }
}
