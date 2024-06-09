using BaseArch.Application.Identity.Interfaces;

namespace Application.Identity.Dtos
{
    public class IdentityUser : IIdentityUser<Guid, string>
    {
        public required Guid Id { get; init; }
        public required string Email { get; init; }
        public required string UserName { get; init; }
        public IEnumerable<string> Roles { get; init; }
    }
}
