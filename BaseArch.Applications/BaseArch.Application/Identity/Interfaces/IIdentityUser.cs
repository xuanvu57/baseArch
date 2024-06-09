namespace BaseArch.Application.Identity.Interfaces
{
    public interface IIdentityUser<TKey, TRole>
    {
        TKey Id { get; init; }

        string Email { get; init; }

        string UserName { get; init; }

        IEnumerable<TRole> Roles { get; init; }
    }
}
