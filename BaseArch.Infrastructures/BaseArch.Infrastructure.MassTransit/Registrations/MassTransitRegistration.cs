using BaseArch.Application.MessageQueues;
using BaseArch.Infrastructure.MassTransit.Implementations;
using BaseArch.Infrastructure.MassTransit.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BaseArch.Infrastructure.MassTransit.Registrations
{
    public static class MassTransitRegistration
    {
        public static void RegisterMassTransit(this IServiceCollection services)
        {
            services.AddOptions<MessageQueuesOptions>()
                .BindConfiguration("MessageQueues")
                .ValidateOnStart();

            var messageQueuesOptions = services.BuildServiceProvider().GetRequiredService<IOptions<MessageQueuesOptions>>();
            if (messageQueuesOptions.Value.InMemoryQueue)
            {
                RegisterMassTransitWithInMemmoryQueue(services, messageQueuesOptions.Value.Retry);
            }
            else if (messageQueuesOptions.Value.RabbitMq != null)
            {
                RegisterMassTransitWithRabbitMq(services, messageQueuesOptions.Value.RabbitMq, messageQueuesOptions.Value.Retry);
            }
        }

        private static void RegisterMassTransitWithInMemmoryQueue(IServiceCollection services, RetryOptions? retryOptions)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(GetAllGenericConsumerTypes());
                x.AddConsumers(GetAllConsumerTypes());

                x.UsingInMemory((context, configure) =>
                {
                    configure.ConfigureEndpoints(
                        context,
                        new KebabCaseEndpointNameFormatter(false));

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

        private static void RegisterMassTransitWithRabbitMq(IServiceCollection services, RabbitMqOptions rabbitMqOptions, RetryOptions? retryOptions)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(GetAllGenericConsumerTypes());
                x.AddConsumers(GetAllConsumerTypes());

                x.UsingRabbitMq((context, configure) =>
                {
                    configure.Host(rabbitMqOptions.Server, "/", c =>
                    {
                        c.Username(rabbitMqOptions.Username);
                        c.Password(rabbitMqOptions.Password);
                    });
                    configure.ConfigureEndpoints(context,
                        new KebabCaseEndpointNameFormatter(false));

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

        private static Type[] GetAllGenericConsumerTypes()
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

        private static Type[] GetAllConsumerTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.ExportedTypes)
                .Where(type =>
                    !type.IsGenericType &&
                    !type.IsAbstract &&
                    !type.FullName!.Contains("MassTransit") &&
                    type.IsAssignableTo(typeof(IConsumer)))
                .ToArray();
        }
    }
}
