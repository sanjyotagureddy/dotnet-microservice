﻿using DotNet.Microservice.Application.Common.Services;

namespace DotNet.Microservice.Application.Common.Commands;

public class UpdateEntityCommand<TEntity> : ICommand
  where TEntity : Entity<Guid>, IAggregateRoot
{
  public UpdateEntityCommand(TEntity entity)
  {
    Entity = entity;
  }

  public TEntity Entity { get; set; }
}

internal class UpdateEntityCommandHandler<TEntity> : ICommandHandler<UpdateEntityCommand<TEntity>>
  where TEntity : Entity<Guid>, IAggregateRoot
{
  private readonly ICrudService<TEntity> _crudService;

  public UpdateEntityCommandHandler(ICrudService<TEntity> crudService)
  {
    _crudService = crudService;
  }

  public async Task HandleAsync(UpdateEntityCommand<TEntity> command, CancellationToken cancellationToken = default)
  {
    await _crudService.UpdateAsync(command.Entity);
  }
}