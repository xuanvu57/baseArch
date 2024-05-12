using BaseArch.Domain.Attributes;
using BaseArch.Domain.Constants;
using BaseArch.Domain.Entities;
using BaseArch.Domain.Enums;
using BaseArch.Domain.Interfaces;
using BaseArch.Domain.Models;
using BaseArch.Infrastructure.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseArch.Infrastructure.EFCore.Repositories
{
    /// <inheritdoc/>
    [DIService(DIServiceLifetime.Scoped)]
    public class BaseRepository<TEntity, TKey, TUserKey>(DbContext dbContext) : IBaseRepository<TEntity, TKey> where TEntity : BaseEntity<TKey, TUserKey>
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
        public async Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false)
        {
            return await GetQueryable(predicate, includeDeletedRecords).CountAsync();
        }

        /// <inheritdoc/>
        public async Task<int> Count(QueryModel queryModel, bool includeDeletedRecords = false)
        {
            var queryable = GetQueryable(null, includeDeletedRecords);

            if (queryModel is not null)
            {
                if (queryModel.FilterQueryModel?.FirstOrDefault() is not null)
                {
                    queryable = queryable.GenerateANDFilterExpression(queryModel.FilterQueryModel);
                }

                if (!string.IsNullOrEmpty(queryModel.SearchQueryModel?.SearchText))
                {
                    queryable = queryable.GenerateORFilterExpression(queryModel.SearchQueryModel.FieldNames, queryModel.SearchQueryModel.SearchText);
                }
            }

            return await queryable.CountAsync();
        }

        /// <inheritdoc/>
        public async Task Create(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await dbSet.AddAsync(entity);
        }

        /// <inheritdoc/>
        public Task CreateMany(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task Delete(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task DeleteMany(IEnumerable<TKey> keys)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false)
        {
            return await GetQueryable(predicate, includeDeletedRecords).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IList<TEntity>> Get(QueryModel queryModel, bool includeDeletedRecords = false)
        {
            var queryable = GetQueryable(null, includeDeletedRecords);

            if (queryModel is not null)
            {
                if (queryModel.FilterQueryModel?.FirstOrDefault() is not null)
                {
                    queryable = queryable.GenerateANDFilterExpression(queryModel.FilterQueryModel);
                }

                if (!string.IsNullOrEmpty(queryModel.SearchQueryModel?.SearchText))
                {
                    queryable = queryable.GenerateORFilterExpression(queryModel.SearchQueryModel.FieldNames, queryModel.SearchQueryModel.SearchText);
                }

                if (queryModel.SortQueryModel is not null && !string.IsNullOrWhiteSpace(queryModel.SortQueryModel.SortBy))
                {
                    queryable = queryModel.SortQueryModel.SortOrder switch
                    {
                        SortOrderConst.Desc => queryable.CustomizedOrderByDescending(queryModel.SortQueryModel.SortBy),
                        _ => queryable.CustomizedOrderBy(queryModel.SortQueryModel.SortBy)
                    };
                }

                if (queryModel.PagingQueryModel is not null)
                {
                    queryable = queryable
                        .Skip((queryModel.PagingQueryModel.PageNumber <= 0 ? 1 : queryModel.PagingQueryModel.PageNumber - 1) * queryModel.PagingQueryModel.PageSize)
                        .Take(queryModel.PagingQueryModel.PageSize);
                }
            }

            return await queryable.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetById(TKey id)
        {
            if (id is null)
                return null;

            return await GetQueryable(e => e.Id.Equals(id)).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task UpdateMany(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
