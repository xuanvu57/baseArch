using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.Extensions
{
    /// <summary>
    /// Register business exception handler
    /// </summary>
    public static class BusinessExceptionHandlerRegistration
    {
        /// <summary>
        /// Add services for one business exception handler with Problem Details response 
        /// </summary>
        /// <typeparam name="TBusinessExceptionHandler">A handler is implemented from <see cref="IExceptionHandler"/></typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddBusinessExceptionHandler<TBusinessExceptionHandler>(this IServiceCollection services) where TBusinessExceptionHandler : class, IExceptionHandler
        {
            services.AddExceptionHandler<TBusinessExceptionHandler>();

            services.AddProblemDetails();
        }

        /// <summary>
        /// Add services for two business exception handlers with Problem Details response
        /// </summary>
        /// <typeparam name="TBusinessExceptionHandler1">A handler is implemented from <see cref="IExceptionHandler"/></typeparam>
        /// <typeparam name="TBusinessExceptionHandler2">A handler is implemented from <see cref="IExceptionHandler"/></typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddBusinessExceptionHandler<TBusinessExceptionHandler1, TBusinessExceptionHandler2>(this IServiceCollection services)
            where TBusinessExceptionHandler1 : class, IExceptionHandler
            where TBusinessExceptionHandler2 : class, IExceptionHandler
        {
            services.AddExceptionHandler<TBusinessExceptionHandler1>();
            services.AddExceptionHandler<TBusinessExceptionHandler2>();

            services.AddProblemDetails();
        }

        /// <summary>
        /// Add services for three business exception handlers with Problem Details response
        /// </summary>
        /// <typeparam name="TBusinessExceptionHandler1">A handler is implemented from <see cref="IExceptionHandler"/></typeparam>
        /// <typeparam name="TBusinessExceptionHandler2">A handler is implemented from <see cref="IExceptionHandler"/></typeparam>
        /// <typeparam name="TBusinessExceptionHandler3">A handler is implemented from <see cref="IExceptionHandler"/></typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddBusinessExceptionHandler<TBusinessExceptionHandler1, TBusinessExceptionHandler2, TBusinessExceptionHandler3>(this IServiceCollection services)
            where TBusinessExceptionHandler1 : class, IExceptionHandler
            where TBusinessExceptionHandler2 : class, IExceptionHandler
            where TBusinessExceptionHandler3 : class, IExceptionHandler
        {
            services.AddExceptionHandler<TBusinessExceptionHandler1>();
            services.AddExceptionHandler<TBusinessExceptionHandler2>();
            services.AddExceptionHandler<TBusinessExceptionHandler3>();

            services.AddProblemDetails();
        }
    }
}
