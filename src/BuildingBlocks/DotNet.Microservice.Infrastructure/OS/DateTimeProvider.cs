using DotNet.Microservice.Common.OS;

namespace DotNet.Microservice.Infrastructure.OS;

public class DateTimeProvider : IDateTimeProvider
{
  public DateTime Now => DateTime.Now;

  public DateTime UtcNow => DateTime.UtcNow;

  public DateTimeOffset OffsetNow => DateTimeOffset.Now;

  public DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;
}