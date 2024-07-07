using MongoDB.Driver;

namespace BaseArch.Infrastructure.MongoDB.DbContext.Interfaces
{
    public interface IMongoDbContext : IDisposable
    {
        IMongoDatabase Database { get; }

        IClientSessionHandle? SessionHandle { get; }

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task CommitAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
