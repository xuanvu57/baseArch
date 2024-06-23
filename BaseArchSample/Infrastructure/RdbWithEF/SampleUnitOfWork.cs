using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.EFCore.UnitOfWork;

namespace Infrastructure.RdbWithEF
{
    [DIService(DIServiceLifetime.Scoped)]
    public class SampleUnitOfWork(IServiceProvider serviceProvider, SampleDbContext dbContext) : UnitOfWork(serviceProvider, dbContext)
    {
    }
}
