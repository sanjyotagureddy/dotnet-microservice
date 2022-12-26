﻿using System;

namespace DotNet.Microservice.Domain.Entities;

public interface ITrackable
{
  byte[] RowVersion { get; set; }

  DateTimeOffset CreatedDateTime { get; set; }

  DateTimeOffset? UpdatedDateTime { get; set; }
}