using BaseArch.Application.MessageQueues;
using BaseArch.Infrastructure.MassTransit.Implementations;
using BaseArch.Infrastructure.MassTransit.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static BaseArch.Infrastructure.MassTransit.Options.MassTransitConstants;

namespace BaseArch.Infrastructure.MassTransit.Registrations
{
    public static class MassTransitRegistration
    {
        public static void RegisterMassTransit(this IServiceCollection services)
        {
            services.AddOptions<MassTransitOptions>()
                .BindConfiguration(MassTransitConstants.MassTransitSection)
                .ValidateOnStart();

            var messageQueuesOptions = services.BuildServiceProvider().GetRequiredService<IOptions<MassTransitOptions>>();
            if (messageQueuesOptions.Value.InMemoryQueue is not null && messageQueuesOptions.Value.InMemoryQueue.Enable)
            {
                RegisterMassTransitWithInMemmoryQueue(services, messageQueuesOptions);
            }
            else if (messageQueuesOptions.Value.RabbitMq is not null && messageQueuesOptions.Value.RabbitMq.Enable)
            {
                RegisterMassTransitWithRabbitMq(services, messageQueuesOptions);
            }
        }

        private static void RegisterMassTransitWithInMemmoryQueue(IServiceCollection services, IOptions<MassTransitOptions> messageQueuesOptions)
        {
            var retryOptions = messageQueuesOptions.Value.Retry;
            services.AddMassTransit(x =>
            {
                x.AddConsumers(CreateAllGenericConsumerTypes());
                x.AddConsumers(GetAllCustomizedConsumerTypes());

                x.UsingInMemory((context, configure) =>
                {
                    configure.ConfigureEndpoints(
                        context,
                        GetEndpointNameFormatter(messageQueuesOptions.Value.EndPointFormatter));

                    if (retryOptions != null)
                    {
                        configure.UseMessageRetry(retryConfigure =>
                        {
                            retryConfigure.Interval(retryOptions.MaxTimes, retryOptions.IntevalInSecond);
                        });
                    }
                });
            });
        }

        private static void RegisterMassTransitWithRabbitMq(IServiceCollection services, IOptions<MassTransitOptions> messageQueuesOptions)
        {
            var rabbitMqOptions = messageQueuesOptions.Value.RabbitMq!;
            var retryOptions = messageQueuesOptions.Value.Retry;
            services.AddMassTransit(x =>
            {
                x.AddConsumers(CreateAllGenericConsumerTypes());
                x.AddConsumers(GetAllCustomizedConsumerTypes());

                x.UsingRabbitMq((context, configure) =>
                {
                    configure.Host(rabbitMqOptions.Server, "/", c =>
                    {
                        c.Username(rabbitMqOptions.Username);
                        c.Password(rabbitMqOptions.Password);
                    });
                    configure.ConfigureEndpoints(context,
                        GetEndpointNameFormatter(messageQueuesOptions.Value.EndPointFormatter));

                    if (retryOptions != null)
                    {
                        configure.UseMessageRetry(retryConfigure =>
                        {
                            retryConfigure.Interval(retryOptions.MaxTimes, retryOptions.IntevalInSecond);
                        });
                    }
                });
            });
        }

        private static IEndpointNameFormatter GetEndpointNameFormatter(EndPointFormatterOptions? endPointFormatterOptions)
        {
            return endPointFormatterOptions?.EndPointFormatterCase switch
            {
                EndPointFormatterCaseEnums.SnakeCase => new SnakeCaseEndpointNameFormatter(endPointFormatterOptions.Prefix, false),
                EndPointFormatterCaseEnums.KebabCase => new KebabCaseEndpointNameFormatter(endPointFormatterOptions.Prefix, false),
                _ => new KebabCaseEndpointNameFormatter("", false)
            };
        }

        private static Type[] CreateAllGenericConsumerTypes()
        {
            var messageTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.ExportedTypes)
                .Where(type =>
                    type.BaseType != null &&
                    type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(BaseEventMessage<>))
                .ToList();

            var consumerTypes = messageTypes.Select((messageType) =>
            {
                return typeof(Consumer<>).MakeGenericType(messageType);
            });

            return consumerTypes.ToArray();
        }

        private static Type[] GetAllCustomizedConsumerTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.ExportedTypes)
                .Where(type =>
                    type.BaseType != null &&
                    type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(DefaultConsumer<>) &&
                    type.FullName != typeof(Consumer<>).FullName)
                .ToArray();
        }
    }
}
