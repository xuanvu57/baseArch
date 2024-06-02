using BaseArch.Domain.DependencyInjection.Interfaces;
using BaseArch.Infrastructure.MassTransit.Registrations;
using BaseArch.Infrastructure.StaticMultilingualProvider.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class DependencyInjection : IDependencyInjection
    {
        public void Register(IServiceCollection services)
        {
            services.AddDbContext<SampleDBContext>(options =>
            {
                options
                    .UseInMemoryDatabase("Sample")
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddStaticMultilingualProviders(["en-US", "vi-VN"]);

            services.RegisterMassTransit();
        }
    }
}
