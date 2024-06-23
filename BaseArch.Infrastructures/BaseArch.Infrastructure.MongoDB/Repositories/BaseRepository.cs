using BaseArch.Application.Identity.Interfaces;
using BaseArch.Application.Models;
using BaseArch.Application.Models.Requests;
using BaseArch.Application.Repositories.Interfaces;
using BaseArch.Domain.Entities;
using BaseArch.Domain.Entities.Interfaces;
using BaseArch.Domain.Timezones.Interfaces;
using BaseArch.Infrastructure.MongoDb.DBContext.Interfaces;
using BaseArch.Infrastructure.MongoDB.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace BaseArch.Infrastructure.MongoDB.Repositories
{
    public class BaseRepository<TEntity, TKey, TUserKey>(IMongoDbContext mongoDbContext, ITokenProvider tokenProvider, IDateTimeProvider dateTimeProvider) : IBaseRepository<TEntity, TKey> where TEntity : BaseEntity<TKey, TUserKey>
    {
        /// <summary>
        /// Collection of TEntity
        /// </summary>
        protected readonly IMongoCollection<TEntity> collection = mongoDbContext.Database.GetCollection<TEntity>(typeof(TEntity).Name);

        private IMongoQueryable<TEntity> GetMongoQueryable(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false)
        {
            var queryable = collection.AsQueryable();

            if (predicate is not null)
                queryable = queryable.Where(predicate);

            if (!includeDeletedRecords && typeof(TEntity).IsAssignableTo(typeof(ISoftDeletable)))
            {
                queryable = queryable.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return queryable;
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable(predicate, includeDeletedRecords).CountAsync(cancellationToken);
        }

        public async Task<int> Count(QueryModel queryModel, bool includeDeletedRecords = false, CancellationToken cancellationToken = default)
        {
            var queryable = GetMongoQueryable(null, includeDeletedRecords);

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

        public async Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable(predicate, includeDeletedRecords).ToListAsync(cancellationToken);
        }

        public async Task<IList<TEntity>> Get(QueryModel queryModel, bool includeDeletedRecords = false, CancellationToken cancellationToken = default)
        {
            var queryable = GetMongoQueryable(null, includeDeletedRecords);

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

        public async Task<TEntity?> GetById(TKey id, CancellationToken cancellationToken = default)
        {
            if (id is null)
                return null;

            return await GetMongoQueryable(e => e.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var createdEntity = entity.SetCreation<TEntity>(tokenProvider.GetUserKeyValue(), dateTimeProvider.GetUtcNow());

            await collection.InsertOneAsync(createdEntity, null, cancellationToken);
        }

        public async Task CreateMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            var createdEntities = entities.Select(e => e.SetCreation<TEntity>(tokenProvider.GetUserKeyValue(), dateTimeProvider.GetUtcNow()));

            await collection.InsertManyAsync(createdEntities, null, cancellationToken);
        }

        public async Task Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var filter = FilterDefinition(x => x.Id.Equals(entity.Id), false);

            if (typeof(TEntity).IsAssignableFrom(typeof(ISoftDeletable)))
            {
                var deletedEntity = entity.SetDeletion<TEntity>(tokenProvider.GetUserKeyValue(), dateTimeProvider.GetUtcNow());

                var updateFilter = Builders<TEntity>.Update
                    .Set(x => x.UpdatedDatetimeUtc, deletedEntity.UpdatedDatetimeUtc)
                    .Set(x => x.UpdatedUserId, deletedEntity.UpdatedUserId)
                    .Set(x => ((ISoftDeletable)x).IsDeleted, true);

                await collection.UpdateOneAsync(filter, updateFilter, cancellationToken: cancellationToken);
            }
            else
            {
                await collection.DeleteOneAsync(filter, cancellationToken);
            }
        }

        public async Task DeleteMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (var entity in entities)
            {
                await Delete(entity, cancellationToken);
            }
        }

        public async Task Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var updatedEntity = entity.SetModification<TEntity>(tokenProvider.GetUserKeyValue(), dateTimeProvider.GetUtcNow());

            var filter = FilterDefinition(x => x.Id.Equals(entity.Id), false);

            await collection.ReplaceOneAsync(filter, updatedEntity, cancellationToken: cancellationToken);
        }

        public async Task UpdateMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (var entity in entities)
            {
                await Update(entity, cancellationToken);
            }
        }

        protected FilterDefinition<TEntity> FilterDefinition(Expression<Func<TEntity, bool>>? predicate = null, bool includeDeletedRecords = false)
        {
            var filterBuilder = Builders<TEntity>.Filter;
            var filterDefinition = filterBuilder.Empty;

            if (!includeDeletedRecords && typeof(TEntity).IsAssignableTo(typeof(ISoftDeletable)))
            {
                filterDefinition &= filterBuilder.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            if (predicate is not null)
                filterDefinition &= filterBuilder.Where(predicate);

            return filterDefinition;
        }

    }
}
