﻿using System.Text.Json;
using Confluent.Kafka;
using DotNet.Microservice.Domain.Infrastructure.MessageBrokers;

namespace DotNet.Microservice.Infrastructure.MessageBrokers.Kafka;

public class KafkaReceiver<T> : IMessageReceiver<T>, IDisposable
{
  private readonly IConsumer<Ignore, string> _consumer;

  public KafkaReceiver(string bootstrapServers, string topic, string groupId)
  {
    var config = new ConsumerConfig
    {
      GroupId = groupId,
      BootstrapServers = bootstrapServers,
      AutoOffsetReset = AutoOffsetReset.Earliest,
    };

    _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    _consumer.Subscribe(topic);
  }

  public void Dispose()
  {
    _consumer.Dispose();
  }

  public void Receive(Action<T, MetaData> action)
  {
    CancellationTokenSource cts = new CancellationTokenSource();
    var cancellationToken = cts.Token;

    Task.Factory.StartNew(() =>
    {
      try
      {
        StartReceiving(action, cancellationToken);
      }
      catch (OperationCanceledException)
      {
        Console.WriteLine("Closing consumer.");
        _consumer.Close();
      }
    });
  }

  private void StartReceiving(Action<T, MetaData> action, CancellationToken cancellationToken)
  {
    while (true)
    {
      try
      {
        var consumeResult = _consumer.Consume(cancellationToken);

        if (consumeResult.IsPartitionEOF)
        {
          continue;
        }

        var message = JsonSerializer.Deserialize<Message<T>>(consumeResult.Message.Value);
        action(message.Data, message.MetaData);
      }
      catch (ConsumeException e)
      {
        Console.WriteLine($"Consume error: {e.Error.Reason}");
      }
    }
  }
}