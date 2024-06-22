using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BaseArch.Domain.Entities
{
    /// <summary>
    /// The base entity with base fields
    /// </summary>
    /// <typeparam name="TKey">Type of entity's key</typeparam>
    /// <typeparam name="TUserKey">Type of entity</typeparam>
    public record BaseEntity<TKey, TUserKey>
    {
        /// <summary>
        /// Key of enitty
        /// </summary>
        [Key]
        [NotNull]
        public required TKey Id { get; init; }

        /// <summary>
        /// Created user id
        /// </summary>
        [NotNull]
        public TUserKey CreatedUserId { get; private init; } = default!;

        /// <summary>
        /// Created date time in Utc
        /// </summary>
        [NotNull]
        public DateTime CreatedDatetimeUtc { get; private init; }

        /// <summary>
        /// Latest updated user id
        /// </summary>
        [NotNull]
        public TUserKey UpdatedUserId { get; private init; } = default!;

        /// <summary>
        /// Latest updated date time in Utc
        /// </summary>
        [NotNull]
        public DateTime UpdatedDatetimeUtc { get; private init; }

        public TEntity SetCreation<TEntity>(object createdUserId, DateTime createdDatetimeUtc) where TEntity : BaseEntity<TKey, TUserKey>
        {
            return (TEntity)this with
            {
                CreatedDatetimeUtc = createdDatetimeUtc,
                CreatedUserId = ConvertToTUserKey(createdUserId),
                UpdatedDatetimeUtc = createdDatetimeUtc,
                UpdatedUserId = ConvertToTUserKey(createdUserId)
            };
        }

        public TEntity SetModification<TEntity>(object updatedUserId, DateTime updatedDatetimeUtc) where TEntity : BaseEntity<TKey, TUserKey>
        {
            return (TEntity)this with
            {
                UpdatedDatetimeUtc = updatedDatetimeUtc,
                UpdatedUserId = ConvertToTUserKey(updatedUserId)
            };
        }

        public TEntity SetDeletion<TEntity>(object deleteUserId, DateTime deletedDatetimeUtc) where TEntity : BaseEntity<TKey, TUserKey>
        {
            return (TEntity)this with
            {
                UpdatedDatetimeUtc = deletedDatetimeUtc,
                UpdatedUserId = ConvertToTUserKey(deleteUserId)
            };
        }

        protected virtual TUserKey ConvertToTUserKey(object value)
        {
            try
            {
                return (TUserKey)Convert.ChangeType(value, typeof(TUserKey));
            }
            catch
            {
                return default!;
            }
        }
    }
}
