using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.EFCore.UnitOfWork;

namespace Infrastructure.RdbWithEF
{
    [DIService(DIServiceLifetime.Scoped)]
    public class SampleEfUnitOfWork(IServiceProvider serviceProvider, SampleEfDbContext dbContext) : UnitOfWork(serviceProvider, dbContext)
    {
    }
}
