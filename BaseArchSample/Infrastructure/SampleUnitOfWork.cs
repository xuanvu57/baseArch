using BaseArch.Domain.Attributes;
using BaseArch.Domain.Enums;
using BaseArch.Infrastructure.EFCore.UnitOfWork;

namespace Infrastructure
{
    [DIService(DIServiceLifetime.Scoped)]
    public class SampleUnitOfWork(IServiceProvider serviceProvider, SampleDBContext dbContext) : UnitOfWork(serviceProvider, dbContext)
    {
    }
}
