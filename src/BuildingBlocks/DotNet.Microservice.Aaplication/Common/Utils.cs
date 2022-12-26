using DotNet.Microservice.Application.Common.Commands;
using DotNet.Microservice.Application.Common.Queries;

namespace DotNet.Microservice.Application.Common;

internal static class Utils
{
  public static bool IsHandlerInterface(Type type)
  {
    if (!type.IsGenericType)
    {
      return false;
    }

    var typeDefinition = type.GetGenericTypeDefinition();

    return typeDefinition == typeof(ICommandHandler<>)
           || typeDefinition == typeof(IQueryHandler<,>);
  }
}