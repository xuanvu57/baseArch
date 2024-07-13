using BaseArch.Infrastructure.MongoDB.DbContext.Interfaces;
using BaseArch.Infrastructure.MongoDB.Options;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.MongoDB.Registrations
{
    /// <summary>
    /// Extension to register MongoDb
    /// </summary>
    public static class MongDbRegistration
    {
        /// <summary>
        /// Add MongoDb context
        /// </summary>
        /// <typeparam name="TDbContext">Type of MongoDb context</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configure">Action for <see cref="MongoDbOptions"/></param>
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
