using MongoDB.Driver;

namespace BaseArch.Infrastructure.MongoDb.DBContext.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
    }
}
