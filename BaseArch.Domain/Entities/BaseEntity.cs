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
        public required TUserKey CreatedUserId { get; init; }

        /// <summary>
        /// Created date time in Utc
        /// </summary>
        [NotNull]
        public DateTime CreatedDatetimeUtc { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Latest updated user id
        /// </summary>
        [NotNull]
        public required TUserKey UpdatedUserId { get; init; }

        /// <summary>
        /// Latest updated date time in Utc
        /// </summary>
        [NotNull]
        public DateTime UpdatedDatetimeUtc { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Identify if the record is (soft) deleted
        /// </summary>
        [NotNull]
        public bool IsDeleted { get; init; } = false;
    }
}
