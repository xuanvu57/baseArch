using BaseArch.Infrastructure.MongoDB.DbContext;
using BaseArch.Infrastructure.MongoDB.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.NoSqlWithMongoDb
{
    public class SampleMongoDbContext(IOptions<MongoDbOptions> options) : MongoDbContext(options)
    {
    }
}
