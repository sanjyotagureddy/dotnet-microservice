using DotNet.Microservice.Domain.Infrastructure.MessageBrokers;

namespace DotNet.Microservice.Infrastructure.MessageBrokers.Fake;

public class FakeReceiver<T> : IMessageReceiver<T>
{
  public void Receive(Action<T, MetaData> action)
  {
    // do nothing
  }
}