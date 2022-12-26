namespace DotNet.Microservice.Domain.Entities;

public interface IHasKey<T>
{
  T Id { get; set; }
}