using BaseArch.Domain.Entities;

namespace Domain.Entities
{
    public record UserEntity(string FirstName, string LastName) : BaseEntity<Guid, Guid>;
}
