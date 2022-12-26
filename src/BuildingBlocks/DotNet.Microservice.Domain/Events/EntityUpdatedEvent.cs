using System;
using DotNet.Microservice.Domain.Entities;

namespace DotNet.Microservice.Domain.Events;

public class EntityUpdatedEvent<T> : IDomainEvent
  where T : Entity<Guid>
{
  public EntityUpdatedEvent(T entity, DateTime eventDateTime)
  {
    Entity = entity;
    EventDateTime = eventDateTime;
  }

  public T Entity { get; }

  public DateTime EventDateTime { get; }
}