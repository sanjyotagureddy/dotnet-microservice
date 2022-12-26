using System.Reflection;
using DotNet.Microservice.Application.Common;
using DotNet.Microservice.Application.Common.Commands;
using DotNet.Microservice.Application.Common.Queries;
using DotNet.Microservice.Application.Common.Services;
using DotNet.Microservice.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet.Microservice.Application;

public static class ApplicationServicesExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    services.AddScoped<Dispatcher>();

    services.AddScoped<IDomainEvents, DomainEvents>()
      .AddScoped(typeof(ICrudService<>), typeof(CrudService<>));

    return services;
  }

  public static IServiceCollection AddMessageHandlers(this IServiceCollection services, Assembly assembly)
  {
    var assemblyTypes = assembly.GetTypes();

    foreach (var type in assemblyTypes)
    {
      var handlerInterfaces = type.GetInterfaces()
        .Where(Utils.IsHandlerInterface)
        .ToList();

      if (handlerInterfaces.Any())
      {
        var handlerFactory = new HandlerFactory(type);
        foreach (var interfaceType in handlerInterfaces)
        {
          services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
        }
      }
    }

    var aggregateRootTypes = assembly.GetTypes().Where(x => x.BaseType == typeof(Entity<Guid>) && x.GetInterfaces().Contains(typeof(IAggregateRoot))).ToList();

    var genericHandlerTypes = new[]
    {
      typeof(GetEntititesQueryHandler<>),
      typeof(GetEntityByIdQueryHandler<>),
      typeof(AddOrUpdateEntityCommandHandler<>),
      typeof(DeleteEntityCommandHandler<>),
    };

    foreach (var aggregateRootType in aggregateRootTypes)
    {
      foreach (var genericHandlerType in genericHandlerTypes)
      {
        var handlerType = genericHandlerType.MakeGenericType(aggregateRootType);
        var handlerInterfaces = handlerType.GetInterfaces();

        var handlerFactory = new HandlerFactory(handlerType);
        foreach (var interfaceType in handlerInterfaces)
        {
          services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
        }
      }
    }

    return services;
  }
}