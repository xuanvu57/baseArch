using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.EFCore.UnitOfWork;

namespace Infrastructure
{
    [DIService(DIServiceLifetime.Scoped)]
    public class SampleUnitOfWork(IServiceProvider serviceProvider, SampleDBContext dbContext) : UnitOfWork(serviceProvider, dbContext)
    {
    }
}
