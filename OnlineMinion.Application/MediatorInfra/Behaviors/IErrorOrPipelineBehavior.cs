using ErrorOr;
using JetBrains.Annotations;
using MediatR;

namespace OnlineMinion.Application.MediatorInfra.Behaviors;

[UsedImplicitly]
public interface IErrorOrPipelineBehavior<in TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr;
