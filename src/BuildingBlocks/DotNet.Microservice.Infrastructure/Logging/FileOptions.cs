using Serilog.Events;

namespace DotNet.Microservice.Infrastructure.Logging;

public class FileOptions
{
  public LogEventLevel MinimumLogEventLevel { get; set; }
}