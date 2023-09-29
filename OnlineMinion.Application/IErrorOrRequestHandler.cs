using ErrorOr;
using MediatR;

namespace OnlineMinion.Application;

public interface IErrorOrRequestHandler<in TRequest, TResult>
    : IRequestHandler<TRequest, ErrorOr<TResult>>
    where TRequest : IRequest<ErrorOr<TResult>>;
