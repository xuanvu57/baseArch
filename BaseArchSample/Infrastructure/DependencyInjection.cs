using BaseArch.Domain.DependencyInjection.Interfaces;
using BaseArch.Infrastructure.EFCore.Registrations;
using BaseArch.Infrastructure.Identity.Registrations;
using BaseArch.Infrastructure.MassTransit.Registrations;
using BaseArch.Infrastructure.StaticMultilingualProvider.Registrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class DependencyInjection : IDependencyInjection
    {
        public void Register(IServiceCollection services)
        {
            services.RegisterEFInterceptor();

            services.AddDbContext<SampleDBContext>((serviceProvider, options) =>
            {
                options
                    .UseInMemoryDatabase("Sample")
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .AddInterceptors(serviceProvider.GetEFInterceptors());
            });

            services.AddStaticMultilingualProviders(["en-US", "vi-VN"]);

            services.RegisterMassTransit();

            services.RegisterIdentity();
        }
    }
}
