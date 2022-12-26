using DotNet.Microservice.Domain.Infrastructure.MessageBrokers;

namespace DotNet.Microservice.Infrastructure.MessageBrokers.Fake;

public class FakeSender<T> : IMessageSender<T>
{
  public Task SendAsync(T message, MetaData metaData = null, CancellationToken cancellationToken = default)
  {
    return Task.CompletedTask;
  }
}