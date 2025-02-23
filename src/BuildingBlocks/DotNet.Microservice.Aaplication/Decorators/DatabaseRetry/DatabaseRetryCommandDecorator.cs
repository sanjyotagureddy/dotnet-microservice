﻿using DotNet.Microservice.Application.Common.Commands;

namespace DotNet.Microservice.Application.Decorators.DatabaseRetry;

[Mapping(Type = typeof(DatabaseRetryAttribute))]
public class DatabaseRetryCommandDecorator<TCommand> : DatabaseRetryDecoratorBase, ICommandHandler<TCommand>
  where TCommand : ICommand
{
  private readonly ICommandHandler<TCommand> _handler;

  public DatabaseRetryCommandDecorator(ICommandHandler<TCommand> handler, DatabaseRetryAttribute options)
  {
    DatabaseRetryOptions = options;
    _handler = handler;
  }

  public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
  {
    await WrapExecutionAsync(() => _handler.HandleAsync(command, cancellationToken));
  }
}