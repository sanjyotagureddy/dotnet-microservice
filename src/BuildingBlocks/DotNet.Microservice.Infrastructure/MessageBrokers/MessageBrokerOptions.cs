using DotNet.Microservice.Infrastructure.MessageBrokers.Kafka;
using DotNet.Microservice.Infrastructure.MessageBrokers.RabbitMQ;

namespace DotNet.Microservice.Infrastructure.MessageBrokers;

public class MessageBrokerOptions
{
  public string Provider { get; set; }

  public RabbitMQOptions RabbitMQ { get; set; }

  public KafkaOptions Kafka { get; set; }

  public bool UsedRabbitMQ()
  {
    return Provider == "RabbitMQ";
  }

  public bool UsedKafka()
  {
    return Provider == "Kafka";
  }

  
  public bool UsedFake()
  {
    return Provider == "Fake";
  }
}