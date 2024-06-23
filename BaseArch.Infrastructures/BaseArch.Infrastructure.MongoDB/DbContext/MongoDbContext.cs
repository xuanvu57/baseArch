using BaseArch.Infrastructure.MongoDb.DBContext.Interfaces;
using BaseArch.Infrastructure.MongoDB.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BaseArch.Infrastructure.MongoDB.DbContext
{
    public abstract class MongoDbContext : IMongoDbContext
    {
        public IMongoDatabase Database { get; }

        protected MongoDbContext(IOptions<MongoDbOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            Database = client.GetDatabase(options.Value.DatabaseName);
        }
    }
}
