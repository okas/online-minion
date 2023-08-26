using ErrorOr;
using MediatR;

namespace OnlineMinion.Common;

public interface IApiRequestHandler<in TRequest, TResult>
    : IRequestHandler<TRequest, ErrorOr<TResult>>
    where TRequest : IRequest<ErrorOr<TResult>>;
