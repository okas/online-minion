using ErrorOr;
using JetBrains.Annotations;
using MediatR;

namespace OnlineMinion.Application.MediatorInfra.Behaviors;

[UsedImplicitly]
public interface IErrorOrPipelineBehavior<in TRequest, TResponse>
    : IPipelineBehavior<TRequest, ErrorOr<TResponse>> where TRequest : notnull { }
