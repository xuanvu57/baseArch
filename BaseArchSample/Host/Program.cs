using BaseArch.Application.CorrelationId;
using BaseArch.Application.Extensions;
using BaseArch.Infrastructure.DependencyInjection.Extensions;
using BaseArch.Infrastructure.gRPC.Extensions;
using BaseArch.Infrastructure.gRPC.Interceptors;
using BaseArch.Infrastructure.Serilog.DestructingPolicies;
using BaseArch.Infrastructure.Serilog.Extensions;
using BaseArch.Infrastructure.StaticMultilingualProvider.Extensions;
using BaseArch.Presentation.RestApi.Extensions;
using Serilog;

namespace Host
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting server...");

                var builder = WebApplication.CreateBuilder(args);
                builder.Configuration
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                builder.Host.UseSerilog((context, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
                    loggerConfiguration.Destructure.With(new SensitiveDataDestructuringPolicy(opt =>
                    {
                        opt.MaskValue = "xxxx";
                        opt.Keywords = ["Method"];
                    }));
                });

                // Add services to the container.
                builder.Services.AddControllers();

                // Add customized services
                builder.Services.AddGrpcServices(option =>
                {
                    option.Interceptors.Add<GrpcRequestResponseLoggingInterceptor>();
                });
                builder.Services.AddServicesForRestApi();
                builder.Services.AddRestApiVersioning(2);
                builder.Services.AddSwagger();
                builder.Services.AddCorrelationIdServices<GuidCorrelationIdProvider>();
                builder.Services.RegisterDependencyInjections();

                var app = builder.Build();

                Log.Information("Register middlewares...");

                // Configure the HTTP request pipeline.
                app.UseCorrelationIdMiddleware();                           //Presentation
                app.UseCustomizedSerilogLogContextMiddleware();             //Infra
                app.UserGlobalExceptionHandlingMiddlewareRegistration();    //Application
                app.UseStaticMultilingualProviders();                       //Infra
                app.UseHttpRequestResponseLoggingMiddleware();              //Presentation
                app.UseSerilogRequestLogging();
                app.UseExceptionHandler();

                if (!app.Environment.IsProduction())
                {
                    app.UseSwaggerMiddleware();
                }
                app.UseHttpsRedirection();
                app.UseAuthorization();

                app.AutoMapGprcServices();
                app.MapControllers();

                Log.Information("Server is ready!");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Server terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
