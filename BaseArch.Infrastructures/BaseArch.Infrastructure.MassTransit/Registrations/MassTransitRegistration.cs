using BaseArch.Application.MessageQueues;
using BaseArch.Infrastructure.MassTransit.Contants;
using BaseArch.Infrastructure.MassTransit.Implementations;
using BaseArch.Infrastructure.MassTransit.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static BaseArch.Infrastructure.MassTransit.Contants.MassTransitConsts;

namespace BaseArch.Infrastructure.MassTransit.Registrations
{
    /// <summary>
    /// Extension to register MassTransit
    /// </summary>
    public static class MassTransitRegistration
    {
        /// <summary>
        /// Register MassTransit service
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void RegisterMassTransit(this IServiceCollection services)
        {
            services.AddOptions<MassTransitOptions>()
                .BindConfiguration(MassTransitConsts.MassTransitSection)
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

        /// <summary>
        /// Register MassTransit with In-Memory queue
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="messageQueuesOptions">Options for <see cref="MassTransitOptions"/></param>
        private static void RegisterMassTransitWithInMemmoryQueue(IServiceCollection services, IOptions<MassTransitOptions> messageQueuesOptions)
        {
            var retryOptions = messageQueuesOptions.Value.Retry;
            services.AddMassTransit(x =>
            {
                x.AddConsumers(GetAllGenericConsumerTypes());
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

        /// <summary>
        /// Register MassTransit with RabbitMq
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="messageQueuesOptions">Options for <see cref="MassTransitOptions"/></param>
        private static void RegisterMassTransitWithRabbitMq(IServiceCollection services, IOptions<MassTransitOptions> messageQueuesOptions)
        {
            var rabbitMqOptions = messageQueuesOptions.Value.RabbitMq!;
            var retryOptions = messageQueuesOptions.Value.Retry;
            services.AddMassTransit(x =>
            {
                x.AddConsumers(GetAllGenericConsumerTypes());
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

        /// <summary>
        /// Get <see cref="IEndpointNameFormatter"/>
        /// </summary>
        /// <param name="endPointFormatterOptions"><see cref="EndPointFormatterOptions"/></param>
        /// <returns><see cref="IEndpointNameFormatter"/></returns>
        private static IEndpointNameFormatter GetEndpointNameFormatter(EndPointFormatterOptions? endPointFormatterOptions)
        {
            return endPointFormatterOptions?.EndPointFormatterCase switch
            {
                EndPointFormatterCaseEnums.SnakeCase => new SnakeCaseEndpointNameFormatter(endPointFormatterOptions.Prefix, false),
                EndPointFormatterCaseEnums.KebabCase => new KebabCaseEndpointNameFormatter(endPointFormatterOptions.Prefix, false),
                _ => new KebabCaseEndpointNameFormatter("", false)
            };
        }

        /// <summary>
        /// Scan and create generic types for auto consumer
        /// </summary>
        /// <returns>List of <see cref="Type"/></returns>
        private static Type[] GetAllGenericConsumerTypes()
        {
            var messageTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                    type.BaseType != null &&
                    type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(BaseEventMessage<>))
                .Distinct()
                .ToList();

            var consumerTypes = messageTypes.Select((messageType) =>
            {
                return typeof(Consumer<>).MakeGenericType(messageType);
            });

            return consumerTypes.ToArray();
        }

        /// <summary>
        /// Scan and get all customized consumer types
        /// </summary>
        /// <returns>List of <see cref="Type"/></returns>
        private static Type[] GetAllCustomizedConsumerTypes()
        {
            var consumerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                    type.BaseType != null &&
                    type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(DefaultConsumer<>) &&
                    type.FullName != typeof(Consumer<>).FullName)
                .Distinct()
                .ToArray();

            return consumerTypes;
        }
    }
}
