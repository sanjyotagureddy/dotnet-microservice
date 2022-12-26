using Microsoft.Extensions.Configuration;

namespace DotNet.Microservice.Infrastructure.Configuration;

public static class ConfigurationCollectionExtensions
{
  public static IConfigurationBuilder AddAppConfiguration(this IConfigurationBuilder configurationBuilder, ConfigurationProviders options)
  {
    if (options?.SqlServer?.IsEnabled ?? false)
    {
      configurationBuilder.AddSqlServer(options.SqlServer);
    }

    return configurationBuilder;
  }
}