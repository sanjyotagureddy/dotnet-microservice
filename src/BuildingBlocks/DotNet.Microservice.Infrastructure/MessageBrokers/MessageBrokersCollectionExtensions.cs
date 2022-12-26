using DotNet.Microservice.Domain.Infrastructure.MessageBrokers;
using DotNet.Microservice.Infrastructure.HealthChecks;
using DotNet.Microservice.Infrastructure.MessageBrokers.Fake;
using DotNet.Microservice.Infrastructure.MessageBrokers.Kafka;
using DotNet.Microservice.Infrastructure.MessageBrokers.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DotNet.Microservice.Infrastructure.MessageBrokers;

public static class MessageBrokersCollectionExtensions
{
  

  public static IServiceCollection AddFakeSender<T>(this IServiceCollection services)
  {
    services.AddSingleton<IMessageSender<T>>(new FakeSender<T>());
    return services;
  }

  public static IServiceCollection AddFakeReceiver<T>(this IServiceCollection services)
  {
    services.AddTransient<IMessageReceiver<T>>(x => new FakeReceiver<T>());
    return services;
  }

  public static IServiceCollection AddKafkaSender<T>(this IServiceCollection services, KafkaOptions options)
  {
    services.AddSingleton<IMessageSender<T>>(new KafkaSender<T>(options.BootstrapServers, options.Topics[typeof(T).Name]));
    return services;
  }

  public static IServiceCollection AddKafkaReceiver<T>(this IServiceCollection services, KafkaOptions options)
  {
    services.AddTransient<IMessageReceiver<T>>(x => new KafkaReceiver<T>(options.BootstrapServers,
      options.Topics[typeof(T).Name],
      options.GroupId));
    return services;
  }

  public static IServiceCollection AddRabbitMQSender<T>(this IServiceCollection services, RabbitMQOptions options)
  {
    services.AddSingleton<IMessageSender<T>>(new RabbitMQSender<T>(new RabbitMQSenderOptions
    {
      HostName = options.HostName,
      UserName = options.UserName,
      Password = options.Password,
      ExchangeName = options.ExchangeName,
      RoutingKey = options.RoutingKeys[typeof(T).Name],
    }));
    return services;
  }

  public static IServiceCollection AddRabbitMQReceiver<T>(this IServiceCollection services, RabbitMQOptions options)
  {
    services.AddTransient<IMessageReceiver<T>>(x => new RabbitMQReceiver<T>(new RabbitMQReceiverOptions
    {
      HostName = options.HostName,
      UserName = options.UserName,
      Password = options.Password,
      ExchangeName = options.ExchangeName,
      RoutingKey = options.RoutingKeys[typeof(T).Name],
      QueueName = options.QueueNames[typeof(T).Name],
      AutomaticCreateEnabled = true,
    }));
    return services;
  }

  public static IServiceCollection AddMessageBusSender<T>(this IServiceCollection services, MessageBrokerOptions options, IHealthChecksBuilder healthChecksBuilder = null, HashSet<string> checkDulicated = null)
  {
    if (options.UsedRabbitMQ())
    {
      services.AddRabbitMQSender<T>(options.RabbitMQ);

      if (healthChecksBuilder != null)
      {
        var name = "Message Broker (RabbitMQ)";

        healthChecksBuilder.AddRabbitMQ(new RabbitMQHealthCheckOptions
          {
            HostName = options.RabbitMQ.HostName,
            UserName = options.RabbitMQ.UserName,
            Password = options.RabbitMQ.Password,
          },
          name: name,
          failureStatus: HealthStatus.Degraded);

        checkDulicated?.Add(name);
      }
    }
    else if (options.UsedKafka())
    {
      services.AddKafkaSender<T>(options.Kafka);

      if (healthChecksBuilder != null)
      {
        var name = "Message Broker (Kafka)";

        if (checkDulicated == null || !checkDulicated.Contains(name))
        {
          healthChecksBuilder.AddKafka(
            bootstrapServers: options.Kafka.BootstrapServers,
            topic: "healthcheck",
            name: name,
            failureStatus: HealthStatus.Degraded);
        }

        checkDulicated?.Add(name);
      }
    }
    
    else if (options.UsedFake())
    {
      services.AddFakeSender<T>();
    }

    return services;
  }

  public static IServiceCollection AddMessageBusReceiver<T>(this IServiceCollection services, MessageBrokerOptions options)
  {
    if (options.UsedRabbitMQ())
    {
      services.AddRabbitMQReceiver<T>(options.RabbitMQ);
    }
    else if (options.UsedKafka())
    {
      services.AddKafkaReceiver<T>(options.Kafka);
    }
    else if (options.UsedFake())
    {
      services.AddFakeReceiver<T>();
    }

    return services;
  }
}