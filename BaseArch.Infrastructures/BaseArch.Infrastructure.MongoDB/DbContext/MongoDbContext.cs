using BaseArch.Infrastructure.MongoDB.DbContext.Interfaces;
using BaseArch.Infrastructure.MongoDB.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BaseArch.Infrastructure.MongoDB.DbContext
{
    public abstract class MongoDbContext : IMongoDbContext
    {
        /// <summary>
        /// identify if object disposed
        /// </summary>
        private bool _disposed = false;

        private IClientSessionHandle? _sessionHandle;

        private readonly IOptions<MongoDbOptions> _options;

        private MongoClient Client { get; init; }

        public IMongoDatabase Database { get; }

        public IClientSessionHandle? SessionHandle { get { return GetSessionHandler(); } }

        protected MongoDbContext(IOptions<MongoDbOptions> options)
        {
            _options = options;

            Client = new MongoClient(options.Value.ConnectionString);
            Database = Client.GetDatabase(options.Value.DatabaseName);
        }

        private IClientSessionHandle? GetSessionHandler()
        {
            if (!_options.Value.AutoTransaction)
                return _sessionHandle;

            _sessionHandle ??= Client.StartSession();

            if (!_sessionHandle.IsInTransaction)
                _sessionHandle.StartTransaction();

            return _sessionHandle;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (!_options.Value.AutoTransaction)
                return;

            _sessionHandle ??= await Client.StartSessionAsync(cancellationToken: cancellationToken);

            if (!_sessionHandle.IsInTransaction)
                _sessionHandle.StartTransaction();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_sessionHandle is not null)
            {
                await _sessionHandle.CommitTransactionAsync(cancellationToken);
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_sessionHandle is not null)
            {
                await _sessionHandle.AbortTransactionAsync(cancellationToken);
            }
        }

        #region Dispose
        /// <summary>
        /// Finalizer
        /// </summary>
        ~MongoDbContext()
        {
            Dispose(false);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disponse the object
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_sessionHandle is not null && _sessionHandle.IsInTransaction)
                {
                    _sessionHandle.AbortTransaction();
                }
                _sessionHandle?.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}
