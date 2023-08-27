using ErrorOr;
using MediatR;

namespace OnlineMinion.Common;

public interface IErrorOrRequestHandler<in TRequest, TResult>
    : IRequestHandler<TRequest, ErrorOr<TResult>>
    where TRequest : IRequest<ErrorOr<TResult>>;
