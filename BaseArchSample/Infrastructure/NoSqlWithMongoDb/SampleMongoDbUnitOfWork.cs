using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.MongoDB.DbContext.Interfaces;
using BaseArch.Infrastructure.MongoDB.UnitOfWork;

namespace Infrastructure.NoSqlWithMongoDb
{
    [DIService(DIServiceLifetime.Scoped)]
    public class SampleMongoDbUnitOfWork(IServiceProvider serviceProvider, IMongoDbContext dbContext) : UnitOfWork(serviceProvider, dbContext)
    {
    }
}
