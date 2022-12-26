using System;

namespace DotNet.Microservice.Domain.Infrastructure.MessageBrokers;

public interface IMessageReceiver<T>
{
  void Receive(Action<T, MetaData> action);
}