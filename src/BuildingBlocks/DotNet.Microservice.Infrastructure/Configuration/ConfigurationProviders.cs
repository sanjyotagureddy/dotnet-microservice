using CryptographyHelper.Certificates;

namespace DotNet.Microservice.Infrastructure.Configuration;

public class ConfigurationProviders
{
  public SqlServerOptions SqlServer { get; set; }
}

public class SqlServerOptions
{
  public bool IsEnabled { get; set; }

  public string ConnectionString { get; set; }

  public string SqlQuery { get; set; }

  public CertificateOption Certificate { get; set; }
}