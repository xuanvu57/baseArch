using BaseArch.Domain.Entities.Interfaces;

namespace BaseArch.Domain.Entities
{
    public record BaseSoftDeletableEntity<TKey, TUserKey> : BaseEntity<TKey, TUserKey>, ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}
