using DotNet.Microservice.Common.OS;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet.Microservice.Infrastructure.OS;

public static class DateTimeProviderExtensions
{
  public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
  {
    _ = services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    return services;
  }
}