using ErrorOr;
using MediatR;

namespace OnlineMinion.RestApi.MediatorInfra.Behaviors;

public interface IErrorOrPipelineBehavior<in TRequest, TResponse>
    : IPipelineBehavior<TRequest, ErrorOr<TResponse>> where TRequest : notnull { }
