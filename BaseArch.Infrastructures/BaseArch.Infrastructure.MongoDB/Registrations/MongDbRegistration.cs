using BaseArch.Infrastructure.MongoDB.DbContext.Interfaces;
using BaseArch.Infrastructure.MongoDB.Options;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.MongoDB.Registrations
{
    public static class MongDbRegistration
    {
        public static void AddMongoDbContext<TDbContext>(this IServiceCollection services, Action<MongoDbOptions> configure) where TDbContext : class, IMongoDbContext
        {
            services.AddOptions<MongoDbOptions>()
                .Configure(configure)
                .Validate(options =>
                {
                    if (string.IsNullOrEmpty(options.ConnectionString) || string.IsNullOrEmpty(options.DatabaseName))
                        return false;

                    return true;
                }, "MongoDbOptions is invalid");

            services.AddSingleton<IMongoDbContext, TDbContext>();
        }
    }
}
