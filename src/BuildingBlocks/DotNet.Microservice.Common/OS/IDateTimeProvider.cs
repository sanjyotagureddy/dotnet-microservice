﻿namespace DotNet.Microservice.Common.OS;

public interface IDateTimeProvider
{
  DateTime Now { get; }

  DateTime UtcNow { get; }

  DateTimeOffset OffsetNow { get; }

  DateTimeOffset OffsetUtcNow { get; }
}