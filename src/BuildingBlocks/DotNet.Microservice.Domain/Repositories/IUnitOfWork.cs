﻿using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Microservice.Domain.Repositories;

public interface IUnitOfWork
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

  Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);

  Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}